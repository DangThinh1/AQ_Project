using AQ_PGW.Core.ViewModel;
using Microsoft.Extensions.Options;
using PayPal.Api;
using PaypalExpressCheckout.BusinessLogic.ConfigOptions;
using PaypalExpressCheckout.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace PaypalExpressCheckout.BusinessLogic
{
    public class PaypalServices : IPaypalServices
    {
        private readonly PayPalAuthOptions _options;

        public PaypalServices(IOptions<PayPalAuthOptions> options)
        {
            _options = options.Value;
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(_options.PayPalClientId, _options.PayPalClientSecret).GetAccessToken());

            var paymentExecution = new PaymentExecution() { payer_id = payerId };

            var executedPayment = new Payment() { id = paymentId }.Execute(apiContext, paymentExecution);

            return executedPayment;
        }

        public Payment CreatePayment(TransactionProcessParamsItems trans, string intent = "sale")
        {
            var conf = new Dictionary<string, string>()
            {
                {BaseConstants.ClientId,_options.PayPalClientId},
                {BaseConstants.ClientSecret, _options.PayPalClientSecret}
            };
            var apiContext = new APIContext(new OAuthTokenCredential(_options.PayPalClientId, _options.PayPalClientSecret, conf).GetAccessToken());

            var payment = new Payment()
            {
                intent = intent,
                payer = new Payer() { payment_method = "paypal" },
                transactions = GetTransactionsList(trans),
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = !string.IsNullOrEmpty(trans.TransactionProcess.CancelUrl) ? trans.TransactionProcess.CancelUrl : "https://www.paypal.com/us/smarthelp/article/can-i-cancel-a-paypal-payment-faq637",
                    return_url = trans.TransactionProcess.BackUrl
                }
            };

            var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }

        public Refund RefundPayment(string saleId, string parent_payment)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(_options.PayPalClientId, _options.PayPalClientSecret).GetAccessToken());

            var refund = new Refund()
            {
                parent_payment = parent_payment,
                sale_id = saleId
            };

            var sales = new Sale()
            {
                id = saleId
            };

            var data = sales.Refund(apiContext, refund);
            return data;

        }
        private List<Transaction> GetTransactionsList(TransactionProcessParamsItems Trans)
        {
            var transactionList = new List<Transaction>();
            var currency = Trans.TransactionProcess.Currency;
            var itemsList = new List<Item>();
            foreach (var item in Trans.ItemsPayment)
            {
                itemsList.Add(new Item()
                {
                    name = item.ItemName,
                    currency = currency,
                    price = item.Amount.ToString(),
                    quantity = "1",
                    sku = "sku"
                });
            }
            transactionList.Add(new Transaction()
            {
                description = Trans.TransactionProcess.Description,
                invoice_number = Trans.TransactionProcess.OrderId,
                amount = new Amount()
                {
                    currency = currency,
                    total = Trans.TransactionProcess.OrderAmount.ToString(),
                    details = new Details()
                    {
                        tax = "0",
                        shipping = "0",
                        subtotal = Trans.TransactionProcess.OrderAmount.ToString()
                    }
                },

                item_list = new ItemList()
                {
                    items = itemsList
                }
                //payee = new Payee
                //{
                //    // TODO.. Enter the payee email address here
                //    email = !string.IsNullOrEmpty(Trans.TransactionProcess.EmailPayment) ? Trans.TransactionProcess.EmailPayment : _options.PayPalEmail,
                //    //email = "aqbooking@aqbooking.com",

                //    // TODO.. Enter the merchant id here
                //    merchant_id = ""
                //}
            });

            return transactionList;
        }
    }
}
