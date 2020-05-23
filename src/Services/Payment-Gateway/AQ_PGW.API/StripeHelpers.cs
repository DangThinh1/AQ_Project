using AQ_PGW.Core.Models.DBTables;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using AQ_PGW.Common;
using AQ_PGW.Core.Models.Model;

namespace AQ_PGW.API
{
    public static class StripeHelpers
    {
        private readonly static string ApiKey = "pk_test_GtdVUuADjjQQnRDmAiFE5JIJ00cAaVeZrd";
        private readonly static string SecretKey = "sk_test_aQWwVSvd1CKs1JiAa3MFzOaS00Ifigw1fG";

        public static Charge RequestCharge(Transactions transactionModel, decimal payAmount, string description, bool isCapture = true)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            // Create a Customer:
            var customerOptions = new CustomerCreateOptions
            {
                SourceToken = transactionModel.PaymentCardToken,
                //Email = "paying.user@example.com",
            };
            var customerService = new Stripe.CustomerService();
            Stripe.Customer customer = customerService.Create(customerOptions);

            string PaidAmountString = ConvertDecimalAmountToZeroDecimal(payAmount);

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(PaidAmountString),
                Currency = transactionModel.Currency,
                //SourceId = transactionModel.PaymentCardToken,
                Description = description,
                //ReceiptEmail = "nkl@example.com",
                CustomerId = customer.Id,
                Capture = isCapture,
                Metadata = new Dictionary<string, string>()
                {
                    { "order_id", transactionModel.OrderId },
                }
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            return charge;
        }

        public static Charge RequestChargeWithCustomer(Transactions transactionModel, string customerId, decimal payAmount, string description)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            string PaidAmountString = ConvertDecimalAmountToZeroDecimal(payAmount);

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(PaidAmountString),
                Currency = transactionModel.Currency,
                CustomerId = customerId,
                Description = description,
                //ReceiptEmail = "nkl@example.com",
                //Capture = true,
                Metadata = new Dictionary<string, string>()
                {
                    { "order_id", transactionModel.OrderId },
                }
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            return charge;
        }

        public static Charge CaptureCharge(string chargeId, decimal amount)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            string PaidAmountString = ConvertDecimalAmountToZeroDecimal(amount);

            var options = new ChargeCaptureOptions
            {
                Amount = Convert.ToInt64(PaidAmountString),
            };
            var service = new ChargeService();
            Charge charge = service.Capture(chargeId, options);

            return charge;
        }

        public static StripeList<Charge> GetListCharge(Transactions transactionModel)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            var service = new ChargeService();
            var options = new ChargeListOptions
            {
                Limit = 3,
            };
            var charges = service.List(options);

            return charges;
        }

        public static Charge GetByIdCharge(string id)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            var service = new ChargeService();
            var charge = service.Get(id);

            return charge;
        }
        public static Charge UpdateCharge(string id, Transactions transactionModel)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);


            var options = new ChargeUpdateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", transactionModel.OrderId },
                },
            };
            var chargeService = new ChargeService();
            var charge = chargeService.Update(id, options);
            return charge;
        }
        public static Card CreateCard(string cusId, string TokenCard)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/Card/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            //var TokenOptions = new TokenCreateOptions
            //{
            //    Card = new CreditCardOptions
            //    {
            //        Name = card.Name,
            //        ExpMonth = card.Exp_Month,
            //        Number = card.CardNumber,
            //        ExpYear = card.Exp_Year,
            //        Cvc = card.CVC,
            //        AddressCity = card.City,
            //        AddressLine1 = card.Address1,
            //        AddressLine2 = card.Address2,
            //        AddressCountry = card.Country,
            //        AddressZip = card.ZipCode,
            //    }
            //};
            //var TokenService = new TokenService();          
            //var token = TokenService.Create(TokenOptions);

            var CardOptions = new CardCreateOptions
            {               
                SourceToken = TokenCard
            };
            var Cardservice = new CardService();

            var CardCr = Cardservice.Create(cusId, CardOptions);

            return CardCr;
        }
        public static Card RemoveCard(string cusId, string cardId)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/Card/apikeys
            StripeConfiguration.SetApiKey(SecretKey);
            var service = new CardService();
            var cardDelete = service.Delete(cusId, cardId);
            return cardDelete;
        }
        public static string GetTokenCard(CardModel card)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/Card/apikeys

            StripeConfiguration.SetApiKey(SecretKey);

            var TokenOptions = new TokenCreateOptions
            {
                Card = new CreditCardOptions
                {
                    Name = card.Name,
                    ExpMonth = card.Exp_Month,
                    Number = card.CardNumber,
                    ExpYear = card.Exp_Year,
                    Cvc = card.CVC,
                    AddressCity = card.City,
                    AddressLine1 = card.Address1,
                    AddressLine2 = card.Address2,
                    AddressCountry = card.Country,
                    AddressZip = card.ZipCode,
                }
            };
            var TokenService = new TokenService();
            var token = TokenService.Create(TokenOptions);

            return token.Id;
        }
        public static Customer UpdateCardDefault(string cusId, string cardDefault)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/customers/apikeys
            StripeConfiguration.SetApiKey(SecretKey);


            var options = new CustomerUpdateOptions
            {
               DefaultSource = cardDefault
            };
            var customerService = new CustomerService();
            var CusService = customerService.Update(cusId, options);
            return CusService;
        }
        public static StripeList<Card> GetCardWithCustomer(string cusId)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/customers/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            var options = new CardListOptions
            {
                Limit = 10,
            };
            var service = new CardService();

            var ListCard = service.List(cusId, options);
            return ListCard;
        }

        public static Customer GetCustomerById(string cusId)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/customers/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            var service = new CustomerService();
            var customer = service.Get(cusId);

            return customer;
        }

        public static Refund RefundPayment(string ChargerID)
        {
            StripeConfiguration.SetApiKey(SecretKey);
            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                ChargeId = ChargerID,
            };
            Refund refund = refundService.Create(refundOptions);

            return refund;
        }


        public static string ConvertDecimalAmountToZeroDecimal(decimal amount)
        {
            string amountString = amount.ToString("#.##");
            if (amountString.Contains(".") == false)
                amountString += ".00";
            else if (amountString.Split('.')[1].Length == 1)
            {
                amountString += "0";
            }

            return amountString.Replace(".", "");
        }
    }
}
