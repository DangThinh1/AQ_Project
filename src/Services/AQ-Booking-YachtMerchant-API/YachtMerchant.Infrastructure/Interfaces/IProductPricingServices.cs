using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtMerchantProductPricings;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IProductPricingServices
    {
        BaseResponse<bool> Create(ProductPricingCreateModel model);
        BaseResponse<PagedList<ProductPricingViewModel>> GetAllProductPricingByProductId(int productId, ProductPricingSearchModel model);
    }
}
