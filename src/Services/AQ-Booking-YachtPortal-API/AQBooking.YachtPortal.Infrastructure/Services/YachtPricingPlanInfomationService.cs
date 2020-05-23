using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanInfomation;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtPricingPlanInfomationService : IYachtPricingPlanInfomationService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;
        public YachtPricingPlanInfomationService(AQYachtContext aqYachtContext, IMapper mapper)
        {
            _aqYachtContext = aqYachtContext;
            _mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtPricingPlanInfomationViewModel> GetPricingPlanInfo(string yachtFId, int languageId)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                // var result = _aqYachtContext.YachtPricingPlans.FirstOrDefault(k=> !k.Deleted && k.IsActivated && k.YachtFid==yachtFId);
                var result = (from p in _aqYachtContext.YachtPricingPlans
                              join i in _aqYachtContext.YachtPricingPlanInformations
                              on p.Id equals i.PricingPlanFid
                              where p.Deleted == false
                              && p.IsActivated == true
                              && i.Deleted == false
                              && i.LanguageFid == languageId
                              && p.YachtFid == yachtFIdde
                              && p.EffectiveDate <= DateTime.Now.Date
                              && (p.EffectiveEndDate == null || (p.EffectiveEndDate != null && p.EffectiveEndDate >= DateTime.Now.Date))
                              && p.EffectiveDate == _aqYachtContext.YachtPricingPlans
                                                         .Where(x => x.Deleted == false
                                                              && x.IsActivated == true
                                                              && x.YachtFid == yachtFIdde
                                                            && x.EffectiveDate <= DateTime.Now.Date
                                                              && (x.EffectiveEndDate == null || (x.EffectiveEndDate != null && x.EffectiveEndDate >= DateTime.Now.Date))
                                                         )
                                                         .Select(i => i.EffectiveDate).Max()
                              select _mapper.Map<YachtPricingPlanInformations, YachtPricingPlanInfomationViewModel>(i)
                    ).FirstOrDefault();

                if (result != null)
                    return BaseResponse<YachtPricingPlanInfomationViewModel>.Success(result);
                else
                    return BaseResponse<YachtPricingPlanInfomationViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtPricingPlanInfomationViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
