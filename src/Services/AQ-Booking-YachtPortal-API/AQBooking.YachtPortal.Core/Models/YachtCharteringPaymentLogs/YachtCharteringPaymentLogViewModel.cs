﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs
{
   public class YachtCharteringPaymentLogViewModel
    {
        public string Id { get; set; }
        public string CharteringFid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentBy { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentMethod { get; set; }
        public double PaymentAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int StatusFid { get; set; }
        public string Remark { get; set; }
    }
}
