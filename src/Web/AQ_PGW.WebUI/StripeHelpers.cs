using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.WebUI.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQ_PGW.WebUI
{
    public static class StripeHelpers
    {
        private readonly static string ApiKey = "pk_test_GtdVUuADjjQQnRDmAiFE5JIJ00cAaVeZrd";
        private readonly static string SecretKey = "sk_test_aQWwVSvd1CKs1JiAa3MFzOaS00Ifigw1fG";

        public static Charge RequestCharge(Transactions transactionModel)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(transactionModel.OrderAmount.ToString().Replace(".","").Replace(",","")),
                Currency = transactionModel.Currency == "" ? "usd" : transactionModel.Currency, //payment-menthod
                Description = transactionModel.Description,
                SourceId = "tok_visa",
                ReceiptEmail = "nkl@example.com",
                Metadata = new Dictionary<string, string>()
                {
                    { "order_id", transactionModel.OrderId },
                }
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

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
        public static Charge UpdateWithKeyCharge(string id)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(SecretKey);


            var options = new ChargeUpdateOptions
            {

                Metadata = new Dictionary<string, string>
                              {
                                { "order_id", "11223344" },
                              },
            };
            var chargeService = new ChargeService();
            var charge = chargeService.Update(id, options);
            return charge;
        }
    }
}
