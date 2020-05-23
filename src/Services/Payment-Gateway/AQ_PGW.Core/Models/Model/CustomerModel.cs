using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AQ_PGW.Core.Models.Model
{
    public class CustomerModel
    {

        //
        // Summary:
        //     Current balance, if any, being stored on the customer’s account. If negative,
        //     the customer has credit to apply to the next invoice. If positive, the customer
        //     has an amount owed that will be added to the next invoice. The balance does not
        //     refer to any unpaid invoices; it solely takes into account amounts that have
        //     yet to be successfully applied to any invoice. This balance is only taken into
        //     account for recurring billing purposes (i.e., subscriptions, invoices, invoice
        //     items)
        [JsonProperty("account_balance")]
        public long AccountBalance { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        //
        // Summary:
        //     Whether or not the latest charge for the customer’s latest invoice has failed
        [JsonProperty("delinquent")]
        public bool Delinquent { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool? Deleted { get; set; }
        public string DefaultSourceType { get; set; }

        [JsonIgnore]
        public IPaymentSource DefaultSource { get; set; }
        //
        // Summary:
        //     ID of the default source attached to this customer
        //     You can expand the DefaultSource by setting the ExpandDefaultSource property
        //     on the service to true
        public string DefaultSourceId { get; set; }

        public string Currency { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("sources")]
        public StripeList<IPaymentSource> Sources { get; set; }
    }

    public class CardStripeModel
    {
        public bool? Deleted { get; set; }
        
        public long ExpMonth { get; set; }

        public long ExpYear { get; set; }

        public string Last4 { get; set; }
        public string Name { get; set; }
        public string RecipientId { get; set; }
        public string Description { get; set; }
        public string CvcCheck { get; set; }
        public CustomerModel Customer { get; set; }

        public string Currency { get; set; }

        public string Id { get; set; }        

        public string AddressCity { get; set; }

        public string AddressCountry { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressState { get; set; }

        public string AddressZip { get; set; }
                
        public string Brand { get; set; }
        public string Country { get; set; }
        public string CustomerId { get; set; }
    }
}
