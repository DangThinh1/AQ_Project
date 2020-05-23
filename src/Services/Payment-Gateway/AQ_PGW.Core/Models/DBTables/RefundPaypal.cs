using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AQ_PGW.Core.Models.DBTables
{
    public class RefundPaypal
    {
        public RefundPaypal()
        {
            this.create_time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RefundID { get; set; }
        public string sale_id { get; set; }
        public string state { get; set; }
        public string reason { get; set; }
        public string parent_payment { get; set; }
        public string description { get; set; }
        public string TransID { get; set; }
        public DateTime create_time { get; set; }
    }
}
