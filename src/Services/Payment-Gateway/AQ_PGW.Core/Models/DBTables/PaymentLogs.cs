using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class PaymentLogs
    {
        public PaymentLogs()
        {
            CreatedDate = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string PaymentType { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }
        public string TransID { get; set; }
        public string FunctionName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
