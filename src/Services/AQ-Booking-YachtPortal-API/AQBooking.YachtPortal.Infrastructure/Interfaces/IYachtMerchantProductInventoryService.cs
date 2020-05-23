using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtMerchantProductInventoryService
    {
        BaseResponse<List<YachtMerchantProductInventoriesViewModel>> GetProductInventorByAdditionalFId(int additonalFId);
        BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>> GetProductInventoryPricingByAdditionalFId(string additonalFId);
        BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>> GetPriceOfProductInventoryByArrayOfProductId(List<string> productId);
    }
}
