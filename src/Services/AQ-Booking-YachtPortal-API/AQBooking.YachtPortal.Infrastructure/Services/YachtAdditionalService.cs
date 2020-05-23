using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAdditionalServices;
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
    public class YachtAdditionalService : IYachtAdditionalService
    {
        private readonly AQYachtContext _AQYachtContext;
        private readonly IMapper _mapper;

        public YachtAdditionalService(AQYachtContext AQYachtContext, IMapper mapper)
        {
            this._AQYachtContext = AQYachtContext;
            this._mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtAdditionalPackageViewModel>> GetYachtAddictionalPackageByYachtId(string yachtFId)
        {
            try
            {
                var yachtIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (from a in _AQYachtContext.YachtAdditionalServiceControls
                              join b in _AQYachtContext.YachtAdditionalServices on a.AdditionalServiceFid equals b.Id
                              where a.YachtFid == yachtIdde
                              && a.Deleted == false
                              && a.EffectiveDate <= DateTime.Now.Date
                              && (a.EffectiveEndDate == null || (a.EffectiveEndDate != null && a.EffectiveEndDate >= DateTime.Now.Date))

                              && b.Deleted == false
                              && b.IsActive == true
                              && b.ActiveFrom <= DateTime.Now.Date
                              && (b.ActiveTo == null || (b.ActiveTo != null && b.ActiveTo >= DateTime.Now.Date))

                              select _mapper.Map<YachtAdditionalServices, YachtAdditionalPackageViewModel>(b)

                    ).ToList();

                if (result != null)
                    return BaseResponse<List<YachtAdditionalPackageViewModel>>.Success(result);
                else
                    return BaseResponse<List<YachtAdditionalPackageViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtAdditionalPackageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
