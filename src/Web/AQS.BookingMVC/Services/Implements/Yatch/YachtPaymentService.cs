using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Yatch
{
    public class YachtPaymentService : ServiceBase, IYachtPaymentService
    {
        #region Fields
        private readonly YachtPortalApiUrl _yachtPortalApiUrl;
        private string _baseYatchApiUrl = ApiUrlHelper.YachtPortalApi;
        #endregion

        #region Ctor
        public YachtPaymentService(IOptions<YachtPortalApiUrl> yachtPortalApiUrlOption) : base()
        {
            _yachtPortalApiUrl = yachtPortalApiUrlOption.Value;
        }
        #endregion

        #region Method
        /// <summary>
        /// Get Token
        /// </summary>
        /// <returns>Get token</returns>
        public async Task<BaseResponse<string>> GetToken()
        {
            try
            {
                var url = string.Format(_yachtPortalApiUrl.StripePayment.AuthenticationToken,
                    _yachtPortalApiUrl.StripePayment.UserName,
                    _yachtPortalApiUrl.StripePayment.PassWord);
                url = _basePaymentApi + url;

                var response = await _apiExcute.GetData<string>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Transaction
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="transactionId">Transaction id</param>
        /// <param name="aqToken">AQ token</param>
        /// <returns></returns>
        public async Task<BaseResponse<TResponse>> GetTransaction<TResponse>(string transactionId, string aqToken)
        {
            try
            {
                var url = string.Format((_yachtPortalApiUrl.StripePayment.GetTransaction + _basePaymentApi), transactionId);
                var response = await _apiExcute.GetData<TResponse>(url, token: aqToken);

                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Process Payment
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request">Request data</param>
        /// <param name="aqToken">AQ token</param>
        /// <param name="paymentType">Payment type</param>
        /// <returns></returns>
        public async Task<BaseResponse<TResponse>> ProcessPayment<TRequest, TResponse>(
            TRequest request,
            string aqToken,
            string paymentType)
        {
            try
            {
                var url = _basePaymentApi + _yachtPortalApiUrl.StripePayment.ProccessPaymentStrip + "?PaymentType=" + paymentType;
                var res = new BaseRequest<TRequest>() { RequestData = request };
                var response = await _apiExcute.PostData<TResponse, TRequest>
                    (url, HttpMethodEnum.POST, res, aqToken);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public Task<BaseResponse<TResponse>> SaveCharterInformation<TRequest, TResponse>(TRequest request, string paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<TResponse>> UpdateTransactionStatus<TResponse>(string transactionId, int status, string apToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create Transaction Information
        /// </summary>
        /// <typeparam name="TRequest">Request</typeparam>
        /// <typeparam name="TResponse">Response</typeparam>
        /// <param name="request">Request data</param>
        /// <param name="aqToken">AQ token</param>
        /// <returns>Transaction view</returns>
        public async Task<BaseResponse<TResponse>> CreateTransactionInformation<TRequest, TResponse>(TRequest request, string aqToken)
        {
            try
            {
                var url = (_basePaymentApi + _yachtPortalApiUrl.StripePayment.RequestPaymentStripe);
                var response = await _apiExcute.PostData<TResponse, TRequest>(url, HttpMethodEnum.POST, new BaseRequest<TRequest>(request), aqToken);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Token From Api Payment
        /// </summary>
        /// <typeparam name="TRequest">Request</typeparam>
        /// <typeparam name="TResponse">Response</typeparam>
        /// <param name="request">Request data</param>
        /// <param name="aqToken">AQ token</param>
        /// <returns>Token</returns>
        public async Task<BaseResponse<TResponse>> GetTokenFromApiPayment<TRequest, TResponse>(TRequest request, string aqToken)
        {
            try
            {
                var url = _basePaymentApi + _yachtPortalApiUrl.StripePayment.GetTokenCardStripe;
                var response = await _apiExcute.PostData<TResponse, TRequest>(url, HttpMethodEnum.POST, new BaseRequest<TRequest>(request), aqToken);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<TResponse>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion
    }
}
