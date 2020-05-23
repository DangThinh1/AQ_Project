using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
using AQS.BookingMVC.Areas.Yacht.Models;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Yatch
{
    public class YachtCharteringService : ServiceBase, IYachtCharteringService
    {
        #region Fields
        private readonly YachtPortalApiUrl _yachtPortalApiUrl;
        private string _baseYatchApiUrl = ApiUrlHelper.YachtPortalApi;
        #endregion

        #region Ctor
        public YachtCharteringService(IOptions<YachtPortalApiUrl> yachtPortalApiUrlOption) : base()
        {
            _yachtPortalApiUrl = yachtPortalApiUrlOption.Value;
        }
        #endregion

        /// <summary>
        /// Get Charter Information
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<TResponse>> GetCharterInformation<TResponse>(string uniqueId)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtCharterings.CharteringUniqueId, uniqueId);
                var response = await _apiExcute.GetData<TResponse>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Chartering Payment Log By Chartering UniqueId
        /// </summary>
        /// <param name="charteringUniqueId">Chartering UniqueId</param>
        /// <param name="statusFid">Status id</param>
        /// <returns></returns>
        public async Task<BaseResponse<YachtCharteringPaymentLogViewModel>> GetCharteringPaymentLogByCharteringUniqueId(string charteringUniqueId, int statusFid)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.YachtCharteringPaymentLog.CharteringPaymentLogByCharteringUniqueId, charteringUniqueId, statusFid);
                var response = await _apiExcute.GetData<YachtCharteringPaymentLogViewModel>(url, null);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Save Charter Information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public async Task<BaseResponse<SaveCharterPaymentResponseViewModel>> SaveCharterInformation(YachtSavePackageServiceModel request, string paymentMethod)
        {
            try
            {
                var url = string.Format(_baseYatchApiUrl + _yachtPortalApiUrl.Yatchs.CharterPrivatePayment, paymentMethod);
                var response = await _apiExcute.PostData<SaveCharterPaymentResponseViewModel, YachtSavePackageServiceModel>
                    (url, HttpMethodEnum.POST, new BaseRequest<YachtSavePackageServiceModel>(request));
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<SaveCharterPaymentResponseViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Update Charter Private PaymentLog By CharteringUniqueId
        /// </summary>
        /// <param name="paymentNewModel">Payment model</param>
        /// <param name="charteringUniqueId">Chartering unique id</param>
        /// <param name="apiUrl">Api url</param>
        /// <returns></returns>
        public async Task<BaseResponse<YachtCharteringPaymentLogViewModel>> UpdateCharterPrivatePaymentLogByCharteringUniqueId(YachtCharteringPaymentLogViewModel paymentNewModel, string charteringUniqueId, string apiUrl = "")
        {
            try
            {
                var url = string.Format("{0}{1}{2}{3}",
                    _baseYatchApiUrl,
                    _yachtPortalApiUrl.YachtCharteringPaymentLog.Update,
                    "?charteringUniqueId=",
                    charteringUniqueId);

                var res = new BaseRequest<YachtCharteringPaymentLogViewModel>()
                {
                    RequestData = paymentNewModel
                };
                var response = await _apiExcute.PostData<YachtCharteringPaymentLogViewModel, YachtCharteringPaymentLogViewModel>(url, HttpMethodEnum.POST, res);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharteringPaymentLogViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Update Charter Status
        /// </summary>
        /// <param name="charteringUniqueId">Chartering unique id</param>
        /// <param name="status">Status</param>
        /// <returns></returns>
        public async Task<BaseResponse<SaveCharterPaymentResponseViewModel>> UpdateCharterStatus(string charteringUniqueId, int status)
        {
            try
            {
                var url = (_baseYatchApiUrl + _yachtPortalApiUrl.YachtCharterings.UpdateStatus);
                var res = new BaseRequest<CharteringUpdateStatusModel>()
                {
                    RequestData = new CharteringUpdateStatusModel { UniqueId = charteringUniqueId, StatusFId = status }
                };
                var response = await _apiExcute.PostData<SaveCharterPaymentResponseViewModel, CharteringUpdateStatusModel>(url, HttpMethodEnum.POST, res);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<SaveCharterPaymentResponseViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
