using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
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
    public class YachtCharteringPaymentLogService : IYachtCharteringPaymentLogService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;
        public YachtCharteringPaymentLogService(AQYachtContext searchContext, IMapper mapper)
        {
            _aqYachtContext = searchContext;
            _mapper = mapper;
        }


        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringPaymentLogViewModel> GetCharteringPaymentLogBycharteringFId(string charteringFId, int statusFId = 1)
        {
            YachtCharteringPaymentLogViewModel responsePaymentLog = new YachtCharteringPaymentLogViewModel();
            try
            {
                var resCharteringFId = Terminator.Decrypt(charteringFId).ToDouble();

                responsePaymentLog = _aqYachtContext.YachtCharteringPaymentLogs
                    .Where(k => k.CharteringFid == resCharteringFId && k.StatusFid == statusFId)
                    .Select(i => _mapper.Map<YachtCharteringPaymentLogs, YachtCharteringPaymentLogViewModel>(i)).FirstOrDefault();


                return BaseResponse<YachtCharteringPaymentLogViewModel>.Success(responsePaymentLog);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(responsePaymentLog, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringPaymentLogViewModel> GetCharteringPaymentLogByCharteringUniqueId(string charteringUniqueId, int statusFId)
        {
            YachtCharteringPaymentLogViewModel responsePaymentLog = new YachtCharteringPaymentLogViewModel();
            try
            {

                responsePaymentLog = (from c in _aqYachtContext.YachtCharterings
                                      join p in _aqYachtContext.YachtCharteringPaymentLogs
                                      on c.Id equals p.CharteringFid
                                      where p.StatusFid == statusFId
                                      && c.UniqueId.Trim().ToLower() == charteringUniqueId.Trim().ToLower()
                                      select _mapper.Map<YachtCharteringPaymentLogs, YachtCharteringPaymentLogViewModel>(p)).FirstOrDefault();


                return BaseResponse<YachtCharteringPaymentLogViewModel>.Success(responsePaymentLog);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(responsePaymentLog, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringPaymentLogViewModel> UpdateCharterPrivatePaymentLog(YachtCharteringPaymentLogViewModel paymentNewModel)
        {
            try
            {
                var charteringFIdde = Terminator.Decrypt(paymentNewModel.CharteringFid).ToDouble();
                var responsePaymentLog = _aqYachtContext.YachtCharteringPaymentLogs.FirstOrDefault(x => x.CharteringFid == charteringFIdde);
                if (responsePaymentLog != null)
                {

                    if (!string.IsNullOrEmpty(paymentNewModel.PaymentBy))
                        responsePaymentLog.PaymentBy = paymentNewModel.PaymentBy;

                    if (!string.IsNullOrEmpty(paymentNewModel.PaymentRef))
                        responsePaymentLog.PaymentRef = paymentNewModel.PaymentRef;

                    if (!string.IsNullOrEmpty(paymentNewModel.PaymentMethod))
                        responsePaymentLog.PaymentMethod = paymentNewModel.PaymentMethod;

                    if (!string.IsNullOrEmpty(paymentNewModel.CurrencyCode))
                        responsePaymentLog.CurrencyCode = paymentNewModel.CurrencyCode;

                    if (!string.IsNullOrEmpty(paymentNewModel.CultureCode))
                        responsePaymentLog.CultureCode = paymentNewModel.CultureCode;

                    if (!string.IsNullOrEmpty(paymentNewModel.Remark))
                        responsePaymentLog.Remark = paymentNewModel.Remark;


                    responsePaymentLog.StatusFid = paymentNewModel.StatusFid;

                    _aqYachtContext.SaveChanges();
                }
                return BaseResponse<YachtCharteringPaymentLogViewModel>.Success(_mapper.Map<YachtCharteringPaymentLogs, YachtCharteringPaymentLogViewModel>(responsePaymentLog));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(new YachtCharteringPaymentLogViewModel(), ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharteringPaymentLogViewModel> UpdateCharterPrivatePaymentLogByCharteringUniqueId(YachtCharteringPaymentLogViewModel paymentNewModel, string charteringUniqueId)
        {
            try
            {
                YachtCharteringPaymentLogs responsePaymentLog = new YachtCharteringPaymentLogs();
                YachtCharterings chartering = _aqYachtContext.YachtCharterings.FirstOrDefault(x => x.UniqueId.Trim().ToLower() == charteringUniqueId.Trim().ToLower());
                if (chartering != null)
                {
                    responsePaymentLog = _aqYachtContext.YachtCharteringPaymentLogs.FirstOrDefault(x => x.CharteringFid == chartering.Id);
                    if (responsePaymentLog != null)
                    {

                        if (!string.IsNullOrEmpty(paymentNewModel.PaymentBy))
                            responsePaymentLog.PaymentBy = paymentNewModel.PaymentBy;

                        if (!string.IsNullOrEmpty(paymentNewModel.PaymentRef))
                            responsePaymentLog.PaymentRef = paymentNewModel.PaymentRef;

                        if (!string.IsNullOrEmpty(paymentNewModel.PaymentMethod))
                            responsePaymentLog.PaymentMethod = paymentNewModel.PaymentMethod;

                        if (!string.IsNullOrEmpty(paymentNewModel.CurrencyCode))
                            responsePaymentLog.CurrencyCode = paymentNewModel.CurrencyCode;

                        if (!string.IsNullOrEmpty(paymentNewModel.CultureCode))
                            responsePaymentLog.CultureCode = paymentNewModel.CultureCode;

                        if (!string.IsNullOrEmpty(paymentNewModel.Remark))
                            responsePaymentLog.Remark = paymentNewModel.Remark;


                        responsePaymentLog.StatusFid = paymentNewModel.StatusFid;

                        _aqYachtContext.SaveChanges();
                    }
                }

                return BaseResponse<YachtCharteringPaymentLogViewModel>.Success(_mapper.Map<YachtCharteringPaymentLogs, YachtCharteringPaymentLogViewModel>(responsePaymentLog));
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(new YachtCharteringPaymentLogViewModel(), ex.Message);
            }
        }
    }
}
