using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtMerchantProductSuppliers;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantProductSupplierServices
    {
        BaseResponse<PagedList<ProductSupplierViewModel>>GetSupplierByVendorId(int vendorId, ProductSupplierSearchModel model);
        BaseResponse<PagedList<ProductSupplierViewModel>> GetSupplierByProductId(int productId, ProductSupplierSearchModel model);
        BaseResponse<bool> Create(ProductSupplierAddorUpdateModel model);
        BaseResponse<bool> Update(ProductSupplierAddorUpdateModel model);
        BaseResponse<bool> Delete(int vendorId, int productId);
    }
}
