using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtMerchants;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtMerchantService
    {
        BaseResponse<List<YachtMerchantViewModel>> GetYachtMerchantsByDisplayNumber(int DisplayNumber = 0, int ImageType = 4);
        BaseResponse<YachtMerchantViewModel> GetYachtMerchantsById(string Id);
        int GetMerchantLogoByMerchantId(int id);
    }
}
