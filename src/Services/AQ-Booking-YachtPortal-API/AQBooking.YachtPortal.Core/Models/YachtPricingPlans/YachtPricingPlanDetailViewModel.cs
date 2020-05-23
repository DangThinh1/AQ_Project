using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtPricingPlans
{
    public class YachtPricingPlanDetailViewModel1
    {
        public long Id { get; set; }
        public int PricingTypeFid { get; set; }
        public string PricingTypeResKey { get; set; }
        public bool ContactOwner { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int RealDayNumber { get; set; }
    }
}
