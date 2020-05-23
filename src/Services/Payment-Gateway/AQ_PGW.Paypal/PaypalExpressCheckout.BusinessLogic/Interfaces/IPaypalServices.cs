using AQ_PGW.Core.ViewModel;
using PayPal.Api;

namespace PaypalExpressCheckout.BusinessLogic.Interfaces
{
    public interface IPaypalServices
    {
        Payment CreatePayment(TransactionProcessParamsItems trans,  string intent = "sale");

        Payment ExecutePayment(string paymentId, string payerId);
        Refund RefundPayment(string saleId, string parent_payment);
    }
}