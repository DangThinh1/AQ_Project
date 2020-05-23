using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtPricingPlanDetail
{
    public class YachtPricingPlanDetailCreateModel
    {
        public long Id { get; set; }
        public int PricingPlanFid { get; set; }
        public int PricingTypeFid { get; set; }
        public string PricingTypeResKey { get; set; }
        public bool ContactOwner { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool Deleted { get; set; }
        
    }
}
