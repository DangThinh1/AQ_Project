using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.Payments
{
    public class YachtTourStripePaymentModel : YachtTourPaymentModel
    {
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public int Exp_Month { get; set; }
        public int Exp_Year { get; set; }
        public string Cvc { get; set; }
        public int PaymentMethod { get; set; }
    }
}
