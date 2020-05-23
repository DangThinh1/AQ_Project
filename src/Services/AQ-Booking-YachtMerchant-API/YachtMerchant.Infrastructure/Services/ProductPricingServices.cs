using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using Omu.ValueInjecter;
using YachtMerchant.Core.Models.YachtMerchantProductPricings;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class ProductPricingServices : ServiceBase, IProductPricingServices
    {
        public ProductPricingServices(YachtOperatorDbContext context) : base(context)
        {
        }

        public BaseResponse<bool> Create(ProductPricingCreateModel model)
        {
            try
            {
                var entity = new YachtMerchantProductPricings();
                entity.InjectFrom(model);
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

        public BaseResponse<PagedList<ProductPricingViewModel>> GetAllProductPricingByProductId(int productId, ProductPricingSearchModel model)
        {

            var pageSize = model.PageSize > 0 ? model.PageSize : 10;
            var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
            var query = _context.YachtMerchantProductPricings
                                 .Where(x => x.ProductFid == productId && x.Deleted == false)
                                 .Select(x => new ProductPricingViewModel
                                 {
                                     Id = x.Id,
                                     ProductFid = productId,
                                     CultureCode = _context.YachtMerchantProductPricings
                                                         .FirstOrDefault(m => m.Deleted == false && m.ProductFid == x.ProductFid).CultureCode,

                                     CurrencyCode = _context.YachtMerchantProductPricings
                                                         .FirstOrDefault(m => m.Deleted == false && m.ProductFid == x.ProductFid).CurrencyCode,
                                     EffectiveDate = x.EffectiveDate,
                                     EffectiveEndDate = x.EffectiveEndDate,
                                     Price = x.Price
                                 }).OrderBy(x => x.EffectiveDate).ThenBy(x => x.EffectiveEndDate);
            var result = new PagedList<ProductPricingViewModel>(query, pageIndex, pageSize);
            return BaseResponse<PagedList<ProductPricingViewModel>>.Success(result);
        }
    }

}
