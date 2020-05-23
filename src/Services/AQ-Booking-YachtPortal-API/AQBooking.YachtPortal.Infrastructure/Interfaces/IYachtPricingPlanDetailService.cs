using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlans;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtPricingPlanDetailService
    {
        BaseResponse<YachtPricingPlanViewModel> GetPricingPlanDetailYachtFId(string yachtFId);
        BaseResponse<YachtPricingPlanDetailViewModel> GetPricingPlanDetailYachtFIdAndPricingTypeFId(string yachtFId = "", int PricingTypeFid = 0);
    }
}
