using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class RelatedTransactionDetails
    {
        public RelatedTransactionDetails()
        {
            create_time = DateTime.Now;
        }
        [Key]
        public Int64 ID { get; set; }
        public string TransID { get; set; }
        public string IdRelatedSale { get; set; }
        public string payment_mode { get; set; }
        public string state { get; set; }
        public string protection_eligibility { get; set; }
        public string protection_eligibility_type { get; set; }
        public string currency { get; set; }
        public decimal totalAmount { get; set; }
        public string parent_payment { get; set; }
        public DateTime create_time { get; set; }
        public DateTime? update_time { get; set; }

    }
}
