using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Models.Model;
using BraintreeHttp;
using PayPal.Core;
using PayPal.v1.Payments;

namespace AQ_PGW.API
{
    public class PayPalHelper
    {
        public PayPalHelper()
        {
        }
        //public static async Task<CreditCard>  CreateCreditCard(CardModel card)
        //{
        //    // ### Api Context
        //    // Pass in a `APIContext` object to authenticate 
        //    // the call and to send a unique request id 
        //    // (that ensures idempotency). The SDK generates
        //    // a request id if you do not pass one explicitly. 
        //    var environment = new SandboxEnvironment("AXBzX3BK0EIKIf87NiLs0FF56f6CBINvplAEI-eGQA5UCzlpzpsu1GwG3Jbz_0UaMcGVn6QF9xifYafT", "EHX5uun_f-IokPZKeziZPr8bP_S7NScvdqpw13mXaaHJz2TSVlNhsY8TjHqM3PTSzeZnDjkjtc4SlvtR");
        //    var client = new PayPalHttpClient(environment);
        //    // Payment Resource
        //    var creditCard = new CreditCard()
        //    {
        //        BillingAddress = new Address()
        //        {
        //            City = card.City,
        //            CountryCode = card.Country,
        //            Line1 = card.Address1,
        //            Line2 = card.Address2,                    
        //        },                
        //        Cvv2 = card.CVC,
        //        ExpireMonth = card.Exp_Month,
        //        ExpireYear = card.Exp_Year,
        //        LastName = card.Name,
        //        Number = card.CardNumber,
        //        Type = card.Type
        //    };
        //    FundingInstrument fundInstrument = new FundingInstrument();
        //    fundInstrument.CreditCard = creditCard;

        //    List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
        //    fundingInstrumentList.Add(fundInstrument);
        //    client.
        //    PaymentCreateRequest request = new PaymentCreateRequest();
        //    request.RequestBody(payment);
        //    // Create a payment using a valid APIContext
        //    try
        //    {
        //        HttpResponse response = await client.Execute(request);
        //        var statusCode = response.StatusCode;
        //        Payment result = response.Result<Payment>();
        //        return result;
        //    }
        //    catch (HttpException httpException)
        //    {
        //        var statusCode = httpException.StatusCode;
        //        var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
        //    }

        //}

        public static async Task<Payment> ExecutePayment(string paymentId, string payerId)
        {

            var environment = new SandboxEnvironment("AXBzX3BK0EIKIf87NiLs0FF56f6CBINvplAEI-eGQA5UCzlpzpsu1GwG3Jbz_0UaMcGVn6QF9xifYafT", "EHX5uun_f-IokPZKeziZPr8bP_S7NScvdqpw13mXaaHJz2TSVlNhsY8TjHqM3PTSzeZnDjkjtc4SlvtR");
            var client = new PayPalHttpClient(environment);
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 

            // Payment Resource
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = "10",
                            Currency = "USD"
                        }
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = "https://example.com/cancel",
                    ReturnUrl = "https://example.com/return"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);
            // Create a payment using a valid APIContext
            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                return result;
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
            }
            return null;

        }

        private static List<Transaction> GetTransactionsList()
        {
            // A transaction defines the contract of a payment
            // what is the payment for and who is fulfilling it. 
            var transactionList = new List<Transaction>();

            // The Payment creation API requires a list of Transaction; 
            // add the created Transaction to a List
            //transactionList.Add(new Transaction()
            //{
            //    description = "Transaction description.",
            //    invoice_number = GetRandomInvoiceNumber(),
            //    amount = new Amount()
            //    {
            //        currency = "USD",
            //        total = "100.00",       // Total must be equal to sum of shipping, tax and subtotal.
            //        details = new Details() // Details: Let's you specify details of a payment amount.
            //        {
            //            tax = "15",
            //            shipping = "10",
            //            subtotal = "75"
            //        }
            //    },
            //    item_list = new ItemList()
            //    {
            //        items = new List<Item>()
            //{
            //    new Item()
            //    {
            //        name = "Item Name",
            //        currency = "USD",
            //        price = "15",
            //        quantity = "5",
            //        sku = "sku"
            //    }
            //}
            //    }
            //});
            return transactionList;
        }

        private static RedirectUrls GetReturnUrls(string baseUrl, string intent)
        {
            var returnUrl = intent == "sale" ? "/Home/PaymentSuccessful" : "/Home/AuthorizeSuccessful";

            // Redirect URLS
            // These URLs will determine how the user is redirected from PayPal 
            // once they have either approved or canceled the payment.
            return new RedirectUrls()
            {
                CancelUrl = baseUrl + "/Home/PaymentCancelled",
                ReturnUrl = baseUrl + returnUrl
            };
        }

    }
}
