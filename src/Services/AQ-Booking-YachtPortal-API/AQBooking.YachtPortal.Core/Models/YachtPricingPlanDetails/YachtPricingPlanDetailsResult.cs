using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails
{
    public class YachtPricingPlanDetailsResult
    {
        public double Price { get; set; }
        public string CultureCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PricingTypeResKey { get; set; }        
    }
}
