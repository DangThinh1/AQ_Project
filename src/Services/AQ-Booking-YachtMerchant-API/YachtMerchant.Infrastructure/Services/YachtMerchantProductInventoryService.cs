using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.DTO;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantProductInventoryService : ServiceBase, IYachtMerchantProductInventoryService
    {
        public YachtMerchantProductInventoryService(YachtOperatorDbContext context) : base(context)
        {
        }

        public BaseResponse<bool> CreateYachtMerchantProductInventory(YachtMerchantProductInventoryCreateModel model)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var date = DateTime.Now;
                    var entity = new YachtMerchantProductInventories();
                    entity.InjectFrom(model);

                    //Create Product
                    entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    entity.IsActiveForSales = true;
                    entity.IsStockControl = true;
                    entity.Deleted = false;
                    entity.CreatedBy = GetUserGuidId();
                    entity.CreatedDate = date;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = date;
                    _context.YachtMerchantProductInventories.Add(entity);
                    _context.SaveChangesAsync().Wait();

                    //Create Product Price of Product
                    if(model.Price > 0)
                    {
                        var entityProductPricing = new YachtMerchantProductPricings();
                        entityProductPricing.InjectFrom(model);
                        entityProductPricing.ProductFid = entity.Id;
                        entityProductPricing.Deleted = false;
                        entityProductPricing.CreatedBy = GetUserGuidId();
                        entityProductPricing.CreatedDate = date;
                        entityProductPricing.LastModifiedBy = GetUserGuidId();
                        entityProductPricing.LastModifiedDate = date;
                        _context.YachtMerchantProductPricings.Add(entityProductPricing);
                    }
                  

                    //Create Product Supplier of Product
                    if (model.VendorFid > 0)
                    {
                        var entityProductSupplier = new YachtMerchantProductSuppliers();
                        entityProductSupplier.ProductFid = entity.Id;
                        entityProductSupplier.VendorFid = model.VendorFid;
                        entityProductSupplier.EffectiveDate = model.EffectiveDateSupplier;
                        entityProductSupplier.EffectiveEndDate = model.EffectiveEndDateSupplier;
                        entityProductSupplier.Remark = model.Remark;
                        entityProductSupplier.Deleted = false;
                        entityProductSupplier.CreatedBy = GetUserGuidId();
                        entityProductSupplier.CreatedDate = date;
                        entityProductSupplier.LastModifiedBy = GetUserGuidId();
                        entityProductSupplier.LastModifiedDate = date;
                        _context.YachtMerchantProductSuppliers.Add(entityProductSupplier);
                    }

                    //Save and Commit Transaction
                    _context.SaveChangesAsync().Wait();
                    tran.Commit();
                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return BaseResponse<bool>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
                }
            }
        }

        public BaseResponse<bool> UpdateYachtMerchantProductInventory(YachtMerchantProductInventoryUpdateModel model)
        {
            try
            {
                var entity = _context
                    .YachtMerchantProductInventories
                    .FirstOrDefault(x => x.Id == model.Id && x.Deleted == false);

                if (entity != null)
                {
                    entity.InjectFrom(model);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.SaveChangesAsync().Wait();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteYachtMerchantProductInventory(int id)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context
                        .YachtMerchantProductInventories
                        .FirstOrDefault(x => x.Id == id && x.Deleted == false);

                    if (entity == null)
                        return BaseResponse<bool>.NotFound(false);
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    var prices = _context.YachtMerchantProductPricings.Where(x => x.ProductFid == id && !x.Deleted).ToList();
                    foreach (var item in prices)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = GetUserGuidId();
                        item.LastModifiedDate = DateTime.Now;
                    }

                    var suppliers = _context.YachtMerchantProductSuppliers.Where(x => x.ProductFid == id && !x.Deleted).ToList();
                    foreach (var item in suppliers)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = GetUserGuidId();
                        item.LastModifiedDate = DateTime.Now;
                    }

                    _context.SaveChangesAsync().Wait();
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

        public BaseResponse<YachtMerchantProductInventoryViewModel> GetYachtMerchantProductInventoryById(int id)
        {
            try
            {
                var result = _context.YachtMerchantProductInventories
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.Deleted == false)
                    .Select(x => new YachtMerchantProductInventoryViewModel()
                    {
                        Id = x.Id,
                        MerchantFid = x.MerchantFid,
                        ProductCode = x.ProductCode,
                        ProductName = x.ProductName,
                        ProductCategoryFid = x.ProductCategoryFid,
                        Description = x.Description,
                        GsttypeFid = x.GsttypeFid,
                        IsActiveForSales = x.IsActiveForSales,
                        IsStockControl = x.IsStockControl,
                        PriceTypeFid = x.PriceTypeFid,
                        ItemUnitFid = x.ItemUnitFid,
                        Quantities = x.Quantities,
                    }).FirstOrDefault();
                return BaseResponse<YachtMerchantProductInventoryViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtMerchantProductInventoryViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<DTODropdownItem>> GetAllYachtMerchantProductPricingByProductId(int productId)
        {
            var query = _context.YachtMerchantProductPricings.Where(x => x.ProductFid == productId && x.Deleted == false)
                        .Select(m => new DTODropdownItem
                        {
                            Value = m.Id.ToString(),
                            Text = _context.YachtMerchantProductInventories.FirstOrDefault(l => l.Id == m.ProductFid).ProductName
                        });
            return BaseResponse<List<DTODropdownItem >>.Success(query.ToList());
        }

        public BaseResponse<bool> CreateProductPricing(ProductPricingCreateOrUpdateModel model)
        {
            try
            {
                var products = _context.YachtMerchantProductPricings.Where(x => x.ProductFid == model.ProductFid && !x.Deleted && x.EffectiveEndDate > DateTime.Now.Date).ToList();

                if (products.Count > 0)
                {
                    var supplierLast = products[products.Count - 1];
                    var isExistedInfo = false;
                    isExistedInfo = CheckdDateTimeInsert(model.EffectiveDate, model.EffectiveEndDate, supplierLast.EffectiveDate, supplierLast.EffectiveEndDate);

                    if (!isExistedInfo)
                        return BaseResponse<bool>.NotFound(false);
                }
                if (model.EffectiveEndDate.HasValue)
                {
                    if (model.EffectiveDate.Date >= model.EffectiveEndDate.Value.Date)
                        return BaseResponse<bool>.BadRequest(false);
                }

                var entity = new YachtMerchantProductPricings();
                entity.ProductFid = model.ProductFid;
                entity.EffectiveDate = model.EffectiveDate;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                entity.Price = model.Price;
                entity.CultureCode = model.CultureCode;
                entity.CurrencyCode = model.CurrencyCode;
                entity.Deleted = false;
                entity.CreatedBy = GetUserGuidId();
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtMerchantProductPricings.Add(entity);
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateProductPricing(ProductPricingCreateOrUpdateModel model)
        {
            try
            {
                var entity = _context.YachtMerchantProductPricings.FirstOrDefault(x => x.Id == model.Id);
                if (entity != null)
                {
                    entity.EffectiveEndDate = model.EffectiveEndDate;
                    entity.Price = model.Price;
                    entity.CultureCode = model.CultureCode == null ? entity.CultureCode : model.CultureCode;
                    entity.CurrencyCode = model.CurrencyCode == null ? entity.CurrencyCode : model.CurrencyCode;
                    entity.LastModifiedDate = DateTime.Now;
                    entity.LastModifiedBy = GetUserGuidId();
                    _context.YachtMerchantProductPricings.Update(entity);
                    _context.SaveChangesAsync().Wait();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteProductPricing(int id)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.YachtMerchantProductPricings.FirstOrDefault(x => x.Id == id);
                    if (entity == null)
                        return BaseResponse<bool>.NotFound(false);
                    entity.Deleted = true;
                    _context.YachtMerchantProductPricings.Update(entity);
                    _context.SaveChangesAsync().Wait();
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

        public BaseResponse<YachtMerchantProductPricingViewModel> GetAllProductPricingByProductId(int productId)
        {
            var result = _context.YachtMerchantProductPricings
                                 .Where(x => x.ProductFid == productId && x.Deleted == false)
                                 .Select(x => new YachtMerchantProductPricingViewModel
                                 {
                                     ProductName = _context.YachtMerchantProductInventories
                                                           .FirstOrDefault(z => z.Id == productId && z.Deleted == false)
                                                           .ProductName,
                                     CultureCode = _context.YachtMerchantProductPricings
                                                         .FirstOrDefault(m => m.Deleted == false && m.ProductFid == x.ProductFid).CultureCode,

                                     CurrencyCode = _context.YachtMerchantProductPricings
                                                         .FirstOrDefault(m => m.Deleted == false && m.ProductFid == x.ProductFid).CurrencyCode,
                                     ListPrice = _context.YachtMerchantProductPricings
                                                         .Where(m => m.Deleted == false && m.ProductFid == x.ProductFid)
                                                         .Select(m => new ProductPricingViewDetailModel
                                                         {
                                                             Id = m.Id,
                                                             Price = m.Price,
                                                             EffectiveDate = m.EffectiveDate,
                                                             EffectiveEndDate = m.EffectiveEndDate,
                                                             CultureCode = m.CultureCode,
                                                             CurrencyCode = m.CurrencyCode
                                                         }).ToList()
                                 }).FirstOrDefault();
            return BaseResponse < YachtMerchantProductPricingViewModel >.Success( result);
        }

        public BaseResponse<PagedList<YachtMerchantProductInventoryViewModel>> GetAllYachtMerchantProductInventory(YachtMerchantProductInventorySearchModel model)
        {
            try
            {
                var query = (from i in _context.YachtMerchantProductInventories
                             .Where(k => !k.Deleted && k.MerchantFid == model.MerchantId
                              && (string.IsNullOrEmpty(model.ProductName) || k.ProductName.Contains(model.ProductName))
                              && (model.CategoryId == 0 || k.ProductCategoryFid == model.CategoryId)
                              && (model.GSTTTypeId == 0 || k.GsttypeFid == model.GSTTTypeId)
                              && (model.PriceTypeId == 0 || k.PriceTypeFid == model.PriceTypeId)
                              && (model.ItemUnitId == 0 || k.ItemUnitFid == model.ItemUnitId)
                             )
                             select new YachtMerchantProductInventoryViewModel()
                             {
                                 Id = i.Id,
                                 Quantities = i.Quantities,
                                 ProductName = i.ProductName,
                                 ProductCode = i.ProductCode,
                                 GsttypeFid = i.GsttypeFid,
                                 PriceTypeFid = i.PriceTypeFid,
                                 ProductCategoryFid = i.ProductCategoryFid,
                                 ProductCategoryResKey = i.ProductCategoryResKey,
                                 PriceTypeResKey = i.PriceTypeResKey,
                                 ItemUnitResKey = i.ItemUnitResKey,
                                 GsttypeResKey = i.GsttypeResKey,
                                 Count = _context.YachtMerchantProductSuppliers.Where(x => x.ProductFid == i.Id && !x.Deleted).Count(),
                                 Price = _context.YachtMerchantProductPricings
                                                 .OrderByDescending(x => x.EffectiveDate)
                                                 .FirstOrDefault(x => x.ProductFid == i.Id) != null? _context.YachtMerchantProductPricings
                                                 .OrderByDescending(x => x.EffectiveDate)
                                                 .FirstOrDefault(x => x.ProductFid == i.Id && !x.Deleted && DateTime.Now.Date >= x.EffectiveDate.Date)
                                                 .Price  : 0,
                                 CurrencyCode = _context.YachtMerchantProductPricings.FirstOrDefault(x => x.ProductFid == i.Id && !x.Deleted).CurrencyCode ?? "",
                                 CultureCode = _context.YachtMerchantProductPricings.FirstOrDefault(x => x.ProductFid == i.Id && !x.Deleted).CultureCode ?? "",
                             }).OrderByDescending(x => x.Id);

                return BaseResponse < PagedList < YachtMerchantProductInventoryViewModel >>.Success( new PagedList<YachtMerchantProductInventoryViewModel>(query, model.PageIndex, model.PageSize));
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtMerchantProductInventoryViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<ProductPricingViewDetailModel> GetPricingById(int id)
        {
            try
            {
                var result = _context.YachtMerchantProductPricings.Where(x => x.Id == id && x.Deleted == false)

                    .Select(x => new ProductPricingViewDetailModel()
                    {
                        Id = x.Id,
                        ProductFid = x.ProductFid,
                        EffectiveDate = x.EffectiveDate,
                        EffectiveEndDate = x.EffectiveEndDate,
                        Price = x.Price,
                        CultureCode = x.CultureCode,
                        CurrencyCode = x.CurrencyCode
                    }).FirstOrDefault();

                return BaseResponse < ProductPricingViewDetailModel > .Success(result);
            }
            catch(Exception ex)
            {
               return  BaseResponse<ProductPricingViewDetailModel>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateProductSupplier(YachtMerchantProductSupplierViewModel model)
        {
            try
            {
                var entity = _context.YachtMerchantProductSuppliers.FirstOrDefault(x => x.ProductFid == model.ProductFid && x.EffectiveDate > DateTime.Now.Date);

                if (entity == null)
                    return BaseResponse<bool>.NotFound(false);
                var isExistedInfo = false;
                if (entity.EffectiveDate != null)
                {
                    if (entity.EffectiveEndDate != null)
                    {
                        isExistedInfo = _context
                           .YachtMerchantProductSuppliers
                           .Any(r => r.Deleted == false && r.ProductFid == entity.ProductFid
                                && r.EffectiveEndDate.GetValueOrDefault().Date > model.EffectiveEndDate.GetValueOrDefault().Date);
                    }
                    else
                    {
                        isExistedInfo = _context.YachtMerchantProductSuppliers
                                                 .Any(r => r.Deleted == false && r.ProductFid == entity.ProductFid
                                                      && r.EffectiveDate.AddDays(1).Date > model.EffectiveEndDate.GetValueOrDefault().Date);
                    }
                }
                if (isExistedInfo)
                    return BaseResponse<bool>.NotFound(false);
                entity.Remark = model.Remark;
                entity.EffectiveEndDate = model.EffectiveEndDate;
                _context.YachtMerchantProductSuppliers.Update(entity);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtMerchantProductSupplierViewModel>> GetProductSupplierByProductId(int productId)
        {
            try
            {
                var entity = _context.YachtMerchantProductSuppliers.Where(x => x.ProductFid == productId && !x.Deleted)
                            .Select(x => new YachtMerchantProductSupplierViewModel
                            {
                                ProductFid = x.ProductFid,
                                VendorFid = x.VendorFid,
                                EffectiveDate = x.EffectiveDate,
                                EffectiveEndDate = x.EffectiveEndDate,
                                Remark = x.Remark,
                                ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(z => z.Id == productId && !x.Deleted).ProductName,
                                VendorName = _context.YachtMerchantProductVendors.FirstOrDefault(m => m.Id == x.VendorFid && !x.Deleted).Name
                            }).OrderBy(x => x.EffectiveDate).ThenBy(x => x.EffectiveDate).ToList();
                return BaseResponse<List<YachtMerchantProductSupplierViewModel>>.Success( entity);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantProductSupplierViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtMerchantProductSupplierViewModel> GetProductSupplierByProductIdTest(int productId)
        {
            try
            {
                var entity = _context.YachtMerchantProductSuppliers.Where(x => x.ProductFid == productId && !x.Deleted)
                            .Select(x => new YachtMerchantProductSupplierViewModel
                            {
                                ProductFid = x.ProductFid,
                                ProductName = _context.YachtMerchantProductInventories.FirstOrDefault(z => z.Id == productId && !x.Deleted).ProductName,
                                ProductCategoryId = _context.YachtMerchantProductInventories.FirstOrDefault(n => n.Id == productId).ProductCategoryFid,
                                ListProductSupplier = _context.YachtMerchantProductSuppliers
                                                         .Where(m => m.Deleted == false && m.ProductFid == x.ProductFid)
                                                         .Select(m => new YachtMerchantProductSupplierDetailModel
                                                         {
                                                             VendorFid = m.VendorFid,
                                                             VendorName = _context.YachtMerchantProductVendors.FirstOrDefault(v => v.Id == m.VendorFid && !x.Deleted).Name,
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

        public BaseResponse<List<YachtMerchantProductInventoryViewModel>> GetAllProductInventory()
        {
            var rs = _context.YachtMerchantProductInventories.AsNoTracking().Where(k => k.Deleted == false)
                .Select(k => new YachtMerchantProductInventoryViewModel()
                {
                    Id = k.Id,
                    ProductName = k.ProductName
                });
            if (rs.Count() > 0)
                return BaseResponse<List<YachtMerchantProductInventoryViewModel>>.Success( rs.ToList());
            else
            {
                return BaseResponse<List<YachtMerchantProductInventoryViewModel>>.Success( new List<YachtMerchantProductInventoryViewModel>());
            }
        }

        private bool CheckdDateTimeInsert(DateTime EffectiveDateModel, DateTime? EffectiveEndDateModel, DateTime EffectiveDateEntity, DateTime? EffectiveEndDateEntity)
        {
            var isExistedInfo = false;

            if (EffectiveDateEntity != null)
            {
                if (EffectiveEndDateEntity != null)
                {
                    if (EffectiveDateModel.Date > EffectiveEndDateEntity.Value.Date)
                    {
                        if (EffectiveEndDateModel.HasValue)
                        {
                            if (EffectiveEndDateModel.Value.Date > EffectiveDateModel.Date)
                            {
                                isExistedInfo = true;
                            }
                        }
                        isExistedInfo = true;
                    }
                }
                else
                {
                    if (EffectiveEndDateModel.HasValue)
                    {
                        if (EffectiveDateModel.Date > EffectiveDateEntity.Date)
                        {
                            if (EffectiveEndDateModel.Value.Date > EffectiveDateModel.Date)
                            {
                                isExistedInfo = true;
                            }
                        }
                    }
                    else
                    {
                        if (EffectiveDateModel.Date > EffectiveDateEntity.Date)
                        {
                            isExistedInfo = true;
                        }
                    }
                }
            }

            return isExistedInfo;
        }

        public BaseResponse<bool> CreateProductSupplier(YachtMerchantProductSupplierViewModel model)
        {
            try
            {
                if (model.EffectiveEndDate.HasValue && model.EffectiveEndDate.Value.Date < model.EffectiveDate.Date)
                {
                    return BaseResponse<bool>.BadRequest(false);
                }
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
                _context.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message:ex.Message,fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtMerchantProductInventoryViewModel>> GetAllProductInventoryByMerchantId(int merchantId, int categoryId)
        {
            var rs = _context.YachtMerchantProductInventories.AsNoTracking().Where(k => !k.Deleted && k.MerchantFid == merchantId
                                                                                && (categoryId == 0 || k.ProductCategoryFid == categoryId))
                           .Select(k => new YachtMerchantProductInventoryViewModel()
                           {
                               Id = k.Id,
                               ProductName = k.ProductName
                           });
            if (rs.Count() > 0)
                return BaseResponse<List<YachtMerchantProductInventoryViewModel>>.Success( rs.ToList());
            else
            {
                return BaseResponse<List<YachtMerchantProductInventoryViewModel>>.Success( new List<YachtMerchantProductInventoryViewModel>());
            }
        }
    }
}