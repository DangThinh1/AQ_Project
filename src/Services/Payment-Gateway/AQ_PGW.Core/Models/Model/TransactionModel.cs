using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.Model
{
    public class TransactionModel
    {
        public DateTime? CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedUser { get; set; }
        
        public decimal? OrderAmount { get; set; }

        public string OrderPaymentType { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string PaymentMethod { get; set; }
        
        public decimal? OrderAmountRemaining { get; set; }

    }
}
