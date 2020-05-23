using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlans;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtPricingPlanDetailService : IYachtPricingPlanDetailService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;
        public YachtPricingPlanDetailService(AQYachtContext aqYachtContext, IMapper mapper)
        {
            _aqYachtContext = aqYachtContext;
            _mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtPricingPlanViewModel> GetPricingPlanDetailYachtFId(string yachtFId)
        {
            try
            {

                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = _aqYachtContext.YachtPricingPlans
                              .Where(p => p.Deleted == false
                                           && p.IsActivated == true
                                           && p.YachtFid == yachtFIdde
                                           && p.EffectiveDate <= DateTime.Now.Date
                                           && (p.EffectiveEndDate == null || (p.EffectiveEndDate != null && p.EffectiveEndDate >= DateTime.Now.Date))
                                           && p.EffectiveDate == _aqYachtContext.YachtPricingPlans
                                                         .Where(x => x.Deleted == false
                                                              && x.IsActivated == true
                                                              && x.YachtFid == yachtFIdde
                                                              && x.EffectiveDate <= DateTime.Now.Date
                                                              && (x.EffectiveEndDate == null || (x.EffectiveEndDate != null && x.EffectiveEndDate >= DateTime.Now.Date))
                                                         ).OrderByDescending(o => o.EffectiveDate)
                                                         .Select(i => i.EffectiveDate).FirstOrDefault()
                                   ).OrderByDescending(or => or.EffectiveDate)
                                   .Select(r => new YachtPricingPlanViewModel
                                   {
                                       Id = Terminator.Encrypt(r.Id.ToString()),
                                       PricingCategoryFid = r.PricingCategoryFid,
                                       PricingCategoryResKey = r.PricingCategoryResKey,
                                       PlanName = r.PlanName,
                                       BasedPortLocation = r.BasedPortLocation,
                                       YachtPortName = r.YachtPortName,
                                       Remark = r.Remark,
                                       Details = r.Details.Select(rs => new YachtPricingPlanDetailViewModel
                                       {
                                           Id = Terminator.Encrypt(rs.Id.ToString()),
                                           PricingTypeFid = rs.PricingTypeFid,
                                           PricingTypeResKey = rs.PricingTypeResKey,
                                           ContactOwner = rs.ContactOwner,
                                           Price = rs.Price,
                                           CultureCode = rs.CultureCode,
                                           CurrencyCode = rs.CurrencyCode,
                                           RealDayNumber = AmountOfPriceType(rs.PricingTypeFid)
                                       }
                                           ).ToList()
                                   }).FirstOrDefault();

                if (result != null)
                    return BaseResponse<YachtPricingPlanViewModel>.Success(result);
                else
                    return BaseResponse<YachtPricingPlanViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                #region log

                #endregion
                return BaseResponse<YachtPricingPlanViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtPricingPlanDetailViewModel> GetPricingPlanDetailYachtFIdAndPricingTypeFId(string yachtFId = "", int PricingTypeFid = 0)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();

                var result = (from p in _aqYachtContext.YachtPricingPlans
                              join d in _aqYachtContext.YachtPricingPlanDetails
                              on p.Id equals d.PricingPlanFid

                              where p.Deleted == false
                                   && p.IsActivated == true
                                   && d.PricingTypeFid == PricingTypeFid
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

                              select _mapper.Map<YachtPricingPlanDetails, YachtPricingPlanDetailViewModel>(d)
                              ).FirstOrDefault();

                if (result != null)
                    return BaseResponse<YachtPricingPlanDetailViewModel>.Success(result);
                else
                    return BaseResponse<YachtPricingPlanDetailViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtPricingPlanDetailViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public int AmountOfPriceType(int value)
        {
            int RealDayNumber = 0;
            if (value == 2)
            {
                RealDayNumber = 1;
            }
            if (value == 3)
            {
                RealDayNumber = 2;
            }
            if (value == 4)
            {
                RealDayNumber = 3;
            }
            if (value == 5)
            {
                RealDayNumber = 4;
            }
            if (value == 6)
            {
                RealDayNumber = 5;
            }
            if (value == 7)
            {
                RealDayNumber = 6;
            }
            if (value == 8)
            {
                RealDayNumber = 7;
            }
            if (value == 9)
            {
                RealDayNumber = 14;
            }
            if (value == 10)
            {
                RealDayNumber = 21;
            }
            if (value == 11)
            {
                RealDayNumber = 28;
            }
            return RealDayNumber;
        }
    }
}
