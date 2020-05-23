using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class TransactionItems
    {
        public Guid ID { get; set; }
        public Guid? TransactionId { get; set; }
        public decimal? PayAmount { get; set; }
        public double? PayPercent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? PayDate { get; set; }
        public string Status { get; set; }
        public int? OrderNo { get; set; }
        public string ReferenceId { get; set; }
    }
}
