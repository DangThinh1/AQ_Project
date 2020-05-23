using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtTourCharter
{
    public class YachtTourCharterResultModel
    {
        public string UniqueId { get; set; }
        public string CultureCode { get; set; }
        public string CurrencyCode { get; set; } 
        public double PrepaidValue { get; set; }
    }
}
