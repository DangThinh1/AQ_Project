using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantProductInventoryService
    {

        BaseResponse<PagedList<YachtMerchantProductInventoryViewModel>> GetAllYachtMerchantProductInventory(YachtMerchantProductInventorySearchModel model);

        BaseResponse<YachtMerchantProductInventoryViewModel> GetYachtMerchantProductInventoryById(int id);

        BaseResponse<bool> CreateYachtMerchantProductInventory(YachtMerchantProductInventoryCreateModel model);

        BaseResponse<bool> UpdateYachtMerchantProductInventory(YachtMerchantProductInventoryUpdateModel model);

        BaseResponse<bool> DeleteYachtMerchantProductInventory(int id);

        BaseResponse<bool> CreateProductPricing(ProductPricingCreateOrUpdateModel model);

        BaseResponse<bool> UpdateProductPricing(ProductPricingCreateOrUpdateModel model);

        BaseResponse<bool> DeleteProductPricing(int id);

        BaseResponse<ProductPricingViewDetailModel> GetPricingById(int id);

        BaseResponse<YachtMerchantProductPricingViewModel> GetAllProductPricingByProductId(int productId);

        BaseResponse<bool> UpdateProductSupplier(YachtMerchantProductSupplierViewModel model);

        BaseResponse<bool> CreateProductSupplier(YachtMerchantProductSupplierViewModel model);

        BaseResponse<List<YachtMerchantProductSupplierViewModel>> GetProductSupplierByProductId(int productId);

        BaseResponse<List<YachtMerchantProductInventoryViewModel>> GetAllProductInventory();

        BaseResponse<YachtMerchantProductSupplierViewModel> GetProductSupplierByProductIdTest(int productId);

        BaseResponse<List<YachtMerchantProductInventoryViewModel>> GetAllProductInventoryByMerchantId(int merchantId, int categoryId);

       

    }
}