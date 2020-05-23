using System.Runtime.Serialization;

namespace AQS.BookingMVC.Models.Payment.StripePayment
{
    #region REQUESTPAYMENT API
    public class RequestPaymentResultStripe
    {
        public RequestPaymentDataStripe Data { get; set; }
    }
    public class RequestPaymentDataStripe
    {
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class RequestPaymentStripe
    {
        public string ID { get; set; }
        public string OrderId { get; set; }
        public decimal OrderAmount { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string BackUrl { get; set; }
        public string Currency { get; set; }
    }
    #endregion

    #region PROCESSPAYMENT API
    public class ResponseProcessPaymentResultStripe
    {
        public ResponseProcessPaymentDataStripe Data { get; set; }
    }
    public class ResponseProcessPaymentDataStripe
    {
        public string FailureMessage { get; set; }
        public string PaymenID { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
    }
    public class RequestProccessPaymentStripe : RequestPaymentStripe
    {
        public string PaymentCardToken { get; set; }
    }
    public class RequestProccessPaymentPayal
    {
        public RequestProccessPaymentPayalObject TransactionProcess { get; set; }
        public PayalDetail ItemPayment { get; set; }
    }
    public class PayalDetail
    {
        public string ItemName { get; set; }
        public string Amount { get; set; }
    }
    public class RequestProccessPaymentPayalObject : RequestProccessPaymentStripe
    {
        public string CancelUrl { get; set; }
    }

    public class RequestExecutePaymentPayPal
    {
        public string TranId { get; set; }
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
        public string PaymentToken { get; set; }
    }
    public class RequestExecutePaymentPayPalApi : RequestExecutePaymentPayPal
    {
        public string CharteringId { get; set; }
    }

    public class ResponseExecutePaymentPayPal
    {
        public ResponseExecutePaymentDataPayPal Data { get; set; }
    }
    public class ResponseExecutePaymentDataPayPal
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
    #endregion

    #region GETTOKENCARD API
    public class ResponseGetTokenResultStripe
    {
        public string Data { get; set; }
    }
    public class RequestGetTokenCardStripe
    {
        public string name { get; set; }
        public string cardNumber { get; set; }
        public int exp_Month { get; set; }
        public int exp_Year { get; set; }
        public string cvc { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
    }
    #endregion

    #region AUTHENTICATION
    public class RequestGetTokenAuthenticationStripe
    {
        public string user { get; set; }
        public string pass { get; set; }
    }
    #endregion

    #region STRANSACTION
    public class ResponseTransactionResultStripe
    {
        public ResponseTransactionDataStripe Data { get; set; }
    }
    public class ResponseTransactionDataStripe
    {
        public string Id { get; set; }
        public string CreateDate { get; set; }
        public string CreatedUser { get; set; }
        public string modifiedDate { get; set; }
        public string modifiedUser { get; set; }
        public string orderId { get; set; }
        public string orderAmount { get; set; }
        public string orderPaymentType { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string paymentMethod { get; set; }
        public string failureMessage { get; set; }
        public string referenceId { get; set; }
        public string orderAmountRemaining { get; set; }
        public string backUrl { get; set; }
        public string paymentCardToken { get; set; }
        public string stripeCustomerId { get; set; }
    }
    #endregion

    #region RESPONSE BASE
    public class ResponseBaseStripe<TResponse>
    {
        public int StatusCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        public TResponse result { get; set; }
        public object ResponseHeader { get; set; }
        public string FullResponseString { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FullMsg { get; internal set; }
        public bool IsSuccessStatusCode => StatusCode == 1;

        public string AQStatusCode { get; set; }
        public string ResourceKey { get; set; }

        public static ResponseBaseStripe<TResponse> Success(
            TResponse data = default(TResponse),
            string message = "Success",
            string resourceKey = "SUCCESS",
            string fullMsg = "",
            string fullResponseString = "")
        {
            return new ResponseBaseStripe<TResponse>()
            {
                StatusCode = 1,
                AQStatusCode = "200",
                result = data,
                ResourceKey = resourceKey,
                Message = message,
                ResponseHeader = null,
                FullResponseString = fullResponseString,
                FullMsg = fullMsg
            };
        }

        public static ResponseBaseStripe<TResponse> InternalServerError(
            TResponse data = default(TResponse),
            string message = "Internal Server Error",
            string resourceKey = "INTERNAL_SERVER_ERROR",
            string fullMsg = "",
            string fullResponseString = "")
        {
            return new ResponseBaseStripe<TResponse>()
            {
                StatusCode = 0,
                AQStatusCode = "500",
                result = data,
                ResourceKey = resourceKey,
                Message = message,
                ResponseHeader = null,
                FullResponseString = fullResponseString,
                FullMsg = fullMsg
            };
        }

        public static ResponseBaseStripe<TResponse> BadRequest(
            TResponse data = default(TResponse),
            string message = "Bad Request",
            string resourceKey = "BAD_REQUEST",
            string fullMsg = "",
            string fullResponseString = "")
        {
            return new ResponseBaseStripe<TResponse>()
            {
                StatusCode = 0,
                AQStatusCode = "400",
                result = data,
                ResourceKey = resourceKey,
                Message = message,
                ResponseHeader = null,
                FullResponseString = fullResponseString,
                FullMsg = fullMsg
            };
        }

        public static ResponseBaseStripe<TResponse> NotFound(
            TResponse data = default(TResponse),
            string message = "Data not found",
            string resourceKey = "DATA_NOT_FOUND",
            string fullMsg = "",
            string fullResponseString = "")
        {
            return new ResponseBaseStripe<TResponse>()
            {
                StatusCode = 0,
                AQStatusCode = "404",
                result = data,
                ResourceKey = resourceKey,
                Message = message,
                ResponseHeader = null,
                FullResponseString = fullResponseString,
                FullMsg = fullMsg
            };
        }

        public static ResponseBaseStripe<TResponse> NoContent(
            TResponse data = default(TResponse),
            string message = "No Content",
            string resourceKey = "NO_CONTENT",
            string fullMsg = "",
            string fullResponseString = "")
        {
            return new ResponseBaseStripe<TResponse>()
            {
                StatusCode = 0,
                AQStatusCode = "204",
                result = data,
                ResourceKey = resourceKey,
                Message = message,
                ResponseHeader = null,
                FullResponseString = fullResponseString,
                FullMsg = fullMsg
            };
        }
    }
    #endregion
}
