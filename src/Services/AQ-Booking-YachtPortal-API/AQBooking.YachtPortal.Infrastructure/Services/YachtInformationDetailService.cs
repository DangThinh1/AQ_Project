using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;
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
    public class YachtInformationDetailService : IYachtInformationDetailService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;

        public YachtInformationDetailService(AQYachtContext aqYachtContext, IMapper mapper)
        {
            _aqYachtContext = aqYachtContext;
            _mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtInformationDetailViewModel> GetInfomationDetailByYachtFId(string yachtFId, int lang)
        {
            try
            {
                var yachtIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (from i in _aqYachtContext.YachtInformations.Where(k => k.YachtFid == yachtIdde && k.IsActivated && (k.ActivatedDate == null || (k.ActivatedDate != null && k.ActivatedDate <= DateTime.Now.Date)) && k.Deleted == false)
                              join d in _aqYachtContext.YachtInformationDetails.Where(k => k.LanguageFid == lang && k.IsActivated && k.Deleted == false && (k.ActivatedDate == null || (k.ActivatedDate != null && k.ActivatedDate <= DateTime.Now.Date)))
                              on i.Id equals d.InformationFid
                              orderby d.ActivatedDate descending
                              select _mapper.Map<YachtInformationDetails, YachtInformationDetailViewModel>(d)
                    ).FirstOrDefault();

                if (result != null)
                    return BaseResponse<YachtInformationDetailViewModel>.Success(result);
                else
                    return BaseResponse<YachtInformationDetailViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtInformationDetailViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
