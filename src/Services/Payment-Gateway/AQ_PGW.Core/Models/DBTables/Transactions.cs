using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class Transactions
    {
        public Guid ID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedUser { get; set; }

        public string OrderId { get; set; }

        public decimal? OrderAmount { get; set; }

        public string OrderPaymentType { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string PaymentMethod { get; set; }

        public string FailureMessage { get; set; }

        public string ReferenceId { get; set; }
        public decimal? OrderAmountRemaining { get; set; }
        public string BackUrl { get; set; }

        public string PaymentCardToken { get; set; }

        public string StripeCustomerId { get; set; }
        public string PaymentTypeAPI { get; set; }

    }
}
