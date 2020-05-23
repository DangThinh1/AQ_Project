using APIHelpers.Response;
using AQBooking.Core.Helpers;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.YachtMerchantProductSuppliers;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantProductSupplierServices : ServiceBase, IYachtMerchantProductSupplierServices
    {
        public YachtMerchantProductSupplierServices(YachtOperatorDbContext context) : base(context)
        {
        }

        public BaseResponse<bool> Create(ProductSupplierAddorUpdateModel model)
        {
            try
            {
                var entity = new YachtMerchantProductSuppliers();
                entity.InjectFrom(model);
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

        public BaseResponse<bool> Delete(int vendorId, int productId)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.YachtMerchantProductSuppliers.FirstOrDefault(x => x.VendorFid == vendorId && x.ProductFid == productId && !x.Deleted);
                    if (entity == null)
                        return BaseResponse<bool>.Success(true);
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

        public BaseResponse<PagedList<ProductSupplierViewModel>> GetSupplierByVendorId(int vendorId, ProductSupplierSearchModel model)
        {
            try
            {
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var query = (from v in _context.YachtMerchantProductVendors
                             join s in _context.YachtMerchantProductSuppliers on v.Id equals s.VendorFid
                             where s.Deleted == false && v.Deleted == false && s.VendorFid == vendorId
                             select new ProductSupplierViewModel()
                             {
                                 ProductFid = s.ProductFid,
                                 VendorName = v.Name,
                                 VendorFid = s.VendorFid,
                                 ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(x => x.Id == s.ProductFid) != null ?
                                    _context.YachtMerchantProductInventories.FirstOrDefault(x => x.Id == s.ProductFid).ProductName : string.Empty,
                                 EffectiveDate = s.EffectiveDate,
                                 EffectiveEndDate = s.EffectiveEndDate,
                                 Remark = s.Remark
                             }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate);
                var result = new PagedList<ProductSupplierViewModel>(query, pageIndex, pageSize);
                return BaseResponse<PagedList<ProductSupplierViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<ProductSupplierViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<ProductSupplierViewModel>> GetSupplierByProductId(int productId, ProductSupplierSearchModel model)
        {
            try
            {
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var query = (from v in _context.YachtMerchantProductInventories
                             join s in _context.YachtMerchantProductSuppliers on v.Id equals s.ProductFid
                             where s.Deleted == false && v.Deleted == false && s.ProductFid == productId
                             select new ProductSupplierViewModel()
                             {
                                 ProductFid = s.ProductFid,
                                 VendorFid = s.VendorFid,
                                 VendorName = _context.YachtMerchantProductVendors.FirstOrDefault(x => x.Id == s.VendorFid) != null ?
                                    _context.YachtMerchantProductVendors.FirstOrDefault(x => x.Id == s.VendorFid).Name : string.Empty,
                                 EffectiveDate = s.EffectiveDate,
                                 EffectiveEndDate = s.EffectiveEndDate,
                                 Remark = s.Remark
                             }).OrderBy(m => m.EffectiveDate).ThenBy(m => m.EffectiveDate);
                var result = new PagedList<ProductSupplierViewModel>(query, pageIndex, pageSize);

                return BaseResponse<PagedList<ProductSupplierViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<ProductSupplierViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(ProductSupplierAddorUpdateModel model)
        {
            try
            {
                var entity = _context.YachtMerchantProductSuppliers.FirstOrDefault(x => x.VendorFid == model.VendorFid && x.ProductFid == model.ProductFid);
                if (entity == null)
                    return BaseResponse<bool>.Success(false);
                entity.InjectFrom(model);
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
    }
}