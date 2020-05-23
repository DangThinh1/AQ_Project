using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class RefundStripe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string FailureReason { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string ChargeId { get; set; }
        public decimal Amount { get; set; }
        public string RefundID { get; set; }
        public string TransID { get; set; }

    }
}
