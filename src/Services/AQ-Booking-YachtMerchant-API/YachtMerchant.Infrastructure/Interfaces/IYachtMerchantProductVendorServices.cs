using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Core.Models.YachtMerchantProductVendors;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantProductVendorServices
    {
        BaseResponse<PagedList<YachtMerchantProductVendorViewModel>> Search(YachtMerchantProductVendorSearchModel model);
        BaseResponse<YachtMerchantProductVendorViewModel> GetYachtMerchantProductVendorById(int id);
        Task<BaseResponse<bool>> CreateYachtMerchantProductVendor(YachtMerchantProductVendorCreateModel model);
        Task<BaseResponse<bool>> UpdateYachtMerchantProductVendor(YachtMerchantProductVendorUpdateModel model);
        Task<BaseResponse<bool>> DeleteYachtMerchantProductVendor(int id);
        Task<BaseResponse<List<YachtMerchantProductVendorViewModel>>> GetAllProductVendorByMerchantId(int merchantId, int categoryId = 0);
        BaseResponse<YachtMerchantProductSupplierViewModel> GetProductSupplierByVendorId(int vendorId);
        BaseResponse<bool> CreateProductSupplier(YachtMerchantProductSupplierViewModel model);
        BaseResponse<bool> DeleteProductByVendor(int vendorId, int ProductId);
    }
}
