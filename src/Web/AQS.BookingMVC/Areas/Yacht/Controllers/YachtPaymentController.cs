using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQS.BookingMVC.Areas.Yacht.Models;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Models.Payment.StripePayment;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Areas.Yacht.Controllers
{
    public class YachtPaymentController : Controller
    {
        private readonly IYachtPaymentService _yachtPaymentService;
        private readonly IYachtCharteringService _yachtCharteringService;
        public YachtPaymentController(
            IYachtPaymentService yachtPaymentService,
            IYachtCharteringService yachtCharteringService)
        {
            _yachtPaymentService = yachtPaymentService;
            _yachtCharteringService = yachtCharteringService;
        }

        #region Action
        /// <summary>
        /// Process Yacht Payment
        /// </summary>
        /// <param name="formData">Json data</param>
        /// <returns>Process success or not</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ProcessYachtPayment([FromBody]YachtPaymentInfo formData)
        {
            var charteringUniqueId = string.Empty;
            var paymentProvider = "STRIPE";
            double totalBillAmount = 0;
            var currencyCode = string.Empty;
            var aqToken = string.Empty;
            var transactionId = string.Empty;
            var paymentToken = string.Empty;
            var stripePaymentRequest = new RequestPaymentStripe();
            var createdUser = string.Empty;

            // Validate Form Data
            var isSuccess = (formData != null) && ModelState.IsValid;

            // Calculate total price and save chartering info
            if (isSuccess)
            {
                charteringUniqueId = await SaveCharterInformation(formData.data, paymentProvider);
                isSuccess = (string.IsNullOrEmpty(charteringUniqueId) == false);
            }

            // Get total price and culture code
            if (isSuccess)
            {
                var yachtCharteringViewModel = await GetCharterInformation(charteringUniqueId);
                totalBillAmount = yachtCharteringViewModel.PrepaidValue;
                currencyCode = yachtCharteringViewModel.CurrencyCode;
            }

            // Get aq token
            if (isSuccess)
            {
                aqToken = await GetAqToken();
                isSuccess = (string.IsNullOrEmpty(aqToken) == false);
            }

            // Create transaction
            if (isSuccess)
            {
                transactionId = await CreateTransaction(stripePaymentRequest, charteringUniqueId, totalBillAmount, currencyCode, aqToken);
                isSuccess = (string.IsNullOrEmpty(transactionId) == false);
            }

            // Get card token from api
            if(isSuccess)
            {
                paymentToken = await GetCardToken(formData, aqToken);
                isSuccess = (string.IsNullOrEmpty(paymentToken) == false);
            }

            // Process payment api
            if(isSuccess)
            {
                var request = new RequestProccessPaymentStripe()
                {
                    ID = stripePaymentRequest.ID,
                    OrderId = charteringUniqueId,
                    OrderAmount = (decimal)totalBillAmount,
                    PaymentMethod = "Full", 
                    Currency = currencyCode,
                    Description = "",
                    BackUrl = "/Yacht",
                    PaymentCardToken = paymentToken
                };
                isSuccess = await ProcessStripePayment(request, aqToken, "STRIPE");
            }

            #region Get current transaction and update status, log
            // Get current transaction
            if(isSuccess)
            {
                var response = await _yachtPaymentService.GetTransaction<ResponseTransactionResultStripe>(transactionId, aqToken);
                isSuccess = response.IsValidResponse();
                createdUser = response.ResponseData.Data.CreatedUser;
            }

            // Update transaction status
            if(isSuccess)
            {
                var response = await _yachtCharteringService.UpdateCharterStatus(charteringUniqueId, Convert.ToInt32(YachtCharterStatusEnum.Paid));
                isSuccess = response.IsValidResponse();
            }

            // Update payment log
            if(isSuccess)
            {
                // Get current payment log
                var response =
                    await _yachtCharteringService.GetCharteringPaymentLogByCharteringUniqueId(charteringUniqueId, Convert.ToInt32(YachtCharterStatusEnum.Waiting));

                isSuccess = response.IsValidResponse();

                if (isSuccess)
                {
                    var paymentLog = response.ResponseData;
                    paymentLog.PaymentBy = createdUser;
                    paymentLog.PaymentRef = transactionId;
                    paymentLog.StatusFid = Convert.ToInt32(YachtCharterStatusEnum.Paid);

                    // Update payment log
                    var resultUpdate =
                        await _yachtCharteringService.UpdateCharterPrivatePaymentLogByCharteringUniqueId(paymentLog, charteringUniqueId);

                    isSuccess = resultUpdate.IsValidResponse();
                }
            }
            #endregion

            return Json(new { Success = isSuccess });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Save Charter Information
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="paymentMethod">Payment method</param>
        /// <returns>Charter unique id</returns>
        private async Task<string> SaveCharterInformation(
            YachtSavePackageServiceModel request,
            string paymentMethod)
        {
            var response = await _yachtCharteringService.SaveCharterInformation(request, paymentMethod);
            var isSuccess = response.IsValidResponse() && (string.IsNullOrEmpty(response.ResponseData.UniqueId) == false);

            return isSuccess
                ? response.ResponseData.UniqueId
                : string.Empty;
        }

        /// <summary>
        /// Get Charter Information
        /// </summary>
        /// <param name="charteringUniqueId">Chartering UniqueId</param>
        /// <returns>Charter Information</returns>
        private async Task<YachtCharteringViewModel> GetCharterInformation(string charteringUniqueId)
        {
            var response = await _yachtCharteringService.GetCharterInformation<YachtCharteringViewModel>(charteringUniqueId);
            var isSuccess = response.IsValidResponse();

            return (isSuccess ? response.ResponseData : null);
        }

        /// <summary>
        /// Get Aq Token
        /// </summary>
        /// <returns>Aq Token</returns>
        private async Task<string> GetAqToken()
        {
            var response = await _yachtPaymentService.GetToken();
            var isSuccess = response.IsValidResponse();

            return isSuccess ? response.ResponseData : string.Empty;
        }

        /// <summary>
        /// Create Transaction
        /// </summary>
        /// <param name="stripePaymentRequest">Stripe request payment</param>
        /// <param name="charteringUniqueId">Chartering Unique Id</param>
        /// <param name="totalBillAmount">Total Bill Amount</param>
        /// <param name="currencyCode">Currency Code</param>
        /// <param name="aqToken">AQ token</param>
        /// <returns>Transaction id</returns>
        private async Task<string> CreateTransaction(
            RequestPaymentStripe stripePaymentRequest,
            string charteringUniqueId,
            double totalBillAmount,
            string currencyCode,
            string aqToken)
        {
            stripePaymentRequest = new RequestPaymentStripe()
            {
                ID = Guid.NewGuid().ToString(),
                OrderId = charteringUniqueId,
                OrderAmount = (decimal)totalBillAmount,
                PaymentMethod = "Full",
                Currency = currencyCode,
                Description = string.Empty,
                BackUrl = string.Empty
            };

            var response = await _yachtPaymentService
                .CreateTransactionInformation<RequestPaymentStripe, RequestPaymentResultStripe>(stripePaymentRequest, aqToken);

            var isSuccess = response.IsValidResponse();

            return (isSuccess ? response.ResponseData.Data.TransactionId : string.Empty);
        }

        /// <summary>
        /// Get Card Token
        /// </summary>
        /// <param name="formData">Form data</param>
        /// <param name="aqToken">Aq token</param>
        /// <returns>Card Token</returns>
        private async Task<string> GetCardToken(YachtPaymentInfo formData, string aqToken)
        {
            var request = new RequestGetTokenCardStripe()
            {
                name = formData.name,
                cardNumber = formData.cardNumber,
                exp_Month = formData.exp_Month,
                exp_Year = formData.exp_Year,
                cvc = formData.cvc,
                address1 = string.Empty,
                address2 = string.Empty,
                province = string.Empty,
                country = string.Empty,
                zipCode = string.Empty
            };

            var response = await _yachtPaymentService
                .GetTokenFromApiPayment<RequestGetTokenCardStripe, ResponseGetTokenResultStripe>(request, aqToken);

            return response.IsValidResponse() ? response.ResponseData.Data : string.Empty;
        }

        /// <summary>
        /// Process Stripe Payment
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="aqToken">Aq token</param>
        /// <param name="paymentType">Payment type</param>
        /// <returns>Success or not</returns>
        private async Task<bool> ProcessStripePayment(RequestProccessPaymentStripe request, string aqToken, string paymentType)
        {
            var response = await _yachtPaymentService
                .ProcessPayment<RequestProccessPaymentStripe, BaseResponse<ResponseProcessPaymentResultStripe>>(request, aqToken, paymentType);
            return response.IsValidResponse();
        }
        #endregion
    }
}