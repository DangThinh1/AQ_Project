using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanInfomation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtPricingPlanInfomationService
    {
        BaseResponse<YachtPricingPlanInfomationViewModel> GetPricingPlanInfo(string yachtFId, int languageId);
    }
}
