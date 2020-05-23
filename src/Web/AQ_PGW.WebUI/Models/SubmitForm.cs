using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQ_PGW.WebUI.Models
{
    public class SubmitForm
    {
        public decimal Amount { get; set; }
        public string Decription { get; set; }
        public string BackUrl { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
    } 
}
