using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtPricingPlan;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtPricingPlansService
    {
        Task<BaseResponse<bool>> CreateYachtPricingPlans(YachtPricingPlanCreateModel model);
        BaseResponse<PagedList<YachtPricingPlanViewModel>> SearchYachtPricingPlan(YachtPricingPlanSearchModel searchModel);
        BaseResponse<bool> DeleteYachtPricingPlans(int Id);
        BaseResponse<bool> UpdateYachtPricingPlans(YachtPricingPlanCreateModel model);
        BaseResponse<YachtPricingPlanViewModel> GetById(int Id);
       
    }
}
