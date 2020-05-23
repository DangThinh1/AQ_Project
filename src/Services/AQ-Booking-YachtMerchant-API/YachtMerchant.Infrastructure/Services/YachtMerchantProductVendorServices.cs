using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Core.Models.YachtMerchantProductVendors;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantProductVendorServices : ServiceBase, IYachtMerchantProductVendorServices
    {
        private readonly IMapper _mapper;

        public YachtMerchantProductVendorServices(IMapper mapper, YachtOperatorDbContext context) : base(context)
        {
            _mapper = mapper;
        }

        public BaseResponse<YachtMerchantProductVendorViewModel> GetYachtMerchantProductVendorById(int id)
        {
            try
            {
                return BaseResponse<YachtMerchantProductVendorViewModel>.Success(_context.YachtMerchantProductVendors.AsNoTracking().Where(x => x.Id == id && x.Deleted == false)
                    .Select(x => _mapper.Map<YachtMerchantProductVendors, YachtMerchantProductVendorViewModel>(x)).FirstOrDefault());
            }
            catch(Exception ex)
            {
                return BaseResponse<YachtMerchantProductVendorViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
            
        }

        public async Task<BaseResponse<bool>> CreateYachtMerchantProductVendor(YachtMerchantProductVendorCreateModel model)
        {
            try
            {
                var entity = new YachtMerchantProductVendors();
                entity = _mapper.Map<YachtMerchantProductVendorCreateModel, YachtMerchantProductVendors>(model);
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                await _context.YachtMerchantProductVendors.AddAsync(entity);
                await _context.SaveChangesAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> UpdateYachtMerchantProductVendor(YachtMerchantProductVendorUpdateModel model)
        {
            try
            {
                var entity = _context.YachtMerchantProductVendors.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).FirstOrDefault();
                var checkSupplier = _context.YachtMerchantProductSuppliers.Any(x => x.VendorFid == entity.Id && !x.Deleted);
                if (entity != null && checkSupplier == false)
                {
                    entity.ProductCategoryFid = model.ProductCategoryFid;
                    entity.ProductCategoryResKey = model.ProductCategoryResKey;
                    entity.Name = model.Name;
                    entity.VendorTypeResKey = model.VendorTypeResKey;
                    entity.VendorTypeFid = model.VendorTypeFid;
                    entity.ContactNo = model.ContactNo;
                    entity.Email = model.Email;
                    entity.Address = model.Address;
                    entity.Remark = model.Remark;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtMerchantProductVendors.Update(entity);
                    await _context.SaveChangesAsync();

                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> DeleteYachtMerchantProductVendor(int id)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var vendor = _context.YachtMerchantProductVendors.FirstOrDefault(x => x.Id == id && !x.Deleted);
                    if (vendor == null)
                        return BaseResponse<bool>.NotFound(false);
                    vendor.Deleted = true;
                    _context.YachtMerchantProductVendors.Update(vendor);

                    var vendors = _context.YachtMerchantProductSuppliers.Where(x => x.VendorFid == id && !x.Deleted).ToList();
                    if (vendors.Count > 0)
                    {
                        foreach (var item in vendors)
                        {
                            item.Deleted = true;
                            _context.YachtMerchantProductSuppliers.Update(item);
                        }
                    }

                    await _context.SaveChangesAsync();
                    tran.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }

        public BaseResponse<PagedList<YachtMerchantProductVendorViewModel>> Search(YachtMerchantProductVendorSearchModel model)
        {
            try
            {
                var query = (from i in _context.YachtMerchantProductVendors
                             .Where(k => !k.Deleted && k.MerchantFid == model.MerchantId
                             && (string.IsNullOrEmpty(model.VendorName) || k.Name.Contains(model.VendorName))
                             && (model.VendorTypeId == 0 || k.VendorTypeFid == model.VendorTypeId)
                             && (model.CategoryId == 0 || k.ProductCategoryFid == model.CategoryId))
                             select new YachtMerchantProductVendorViewModel()
                             {
                                 Name = i.Name,
                                 Id = i.Id,
                                 VendorTypeFid = i.VendorTypeFid,
                                 VendorTypeResKey = i.VendorTypeResKey,
                                 ProductCategoryFid = i.ProductCategoryFid,
                                 ProductCategoryResKey = i.ProductCategoryResKey,
                                 Email = i.Email,
                                 ContactNo = i.ContactNo,
                                 Count = _context.YachtMerchantProductSuppliers.Where(x => x.VendorFid == i.Id && !x.Deleted).Count(),
                             }).OrderByDescending(x => x.Id);
                return BaseResponse<PagedList<YachtMerchantProductVendorViewModel>>.Success( new PagedList<YachtMerchantProductVendorViewModel>(query, model.PageIndex, model.PageSize));
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtMerchantProductVendorViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<List<YachtMerchantProductVendorViewModel>>> GetAllProductVendorByMerchantId(int merchantId, int categoryId = 0)
        {
            var rs = _context.YachtMerchantProductVendors.AsNoTracking().Where(k => !k.Deleted && k.MerchantFid == merchantId && (categoryId == 0 || k.ProductCategoryFid == categoryId))
                .Select(k => new YachtMerchantProductVendorViewModel()
                {
                    Id = k.Id,
                    Name = k.Name
                });
            if (rs.Count() > 0)
                return  BaseResponse<List<YachtMerchantProductVendorViewModel>>.Success(await rs.ToListAsync());
            return BaseResponse<List<YachtMerchantProductVendorViewModel>>.Success(new List<YachtMerchantProductVendorViewModel>());
        }

        public BaseResponse<YachtMerchantProductSupplierViewModel> GetProductSupplierByVendorId(int vendorId)
        {
            try
            {
                var entity = _context.YachtMerchantProductSuppliers.Where(x => x.VendorFid == vendorId && !x.Deleted)
                            .Select(x => new YachtMerchantProductSupplierViewModel
                            {
                                VendorFid = x.VendorFid,
                                VendorName = _context.YachtMerchantProductVendors.FirstOrDefault(z => z.Id == vendorId && !x.Deleted).Name,
                                ProductCategoryId = _context.YachtMerchantProductVendors.FirstOrDefault(n => n.Id == vendorId).ProductCategoryFid,
                                ListProductSupplier = _context.YachtMerchantProductSuppliers
                                                         .Where(m => m.Deleted == false && m.VendorFid == x.VendorFid)
                                                         .Select(m => new YachtMerchantProductSupplierDetailModel
                                                         {
                                                             ProductFid = m.ProductFid,

                                                             ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(v => v.Id == m.ProductFid && !x.Deleted).ProductName,
                                                             EffectiveDate = m.EffectiveDate,
                                                             EffectiveEndDate = m.EffectiveEndDate,
                                                             Remark = m.Remark
                                                         }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate).ToList()
                            }).FirstOrDefault();
                return BaseResponse<YachtMerchantProductSupplierViewModel>.Success(entity);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtMerchantProductSupplierViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> CreateProductSupplier(YachtMerchantProductSupplierViewModel model)
        {
            try
            {
                var entity = new YachtMerchantProductSuppliers();
                entity.ProductFid = model.ProductFid;
                entity.VendorFid = model.VendorFid;
                entity.EffectiveDate = model.EffectiveDate;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.Remark = model.Remark;
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtMerchantProductSuppliers.Add(entity);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteProductByVendor(int vendorId, int ProductId)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.YachtMerchantProductSuppliers.FirstOrDefault(x => x.VendorFid == vendorId && x.ProductFid == ProductId && !x.Deleted);
                    if (entity == null)
                        return BaseResponse<bool>.NotFound(false);
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;
                    _context.YachtMerchantProductSuppliers.Update(entity);

                    _context.SaveChanges();
                    tran.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }
    }
}