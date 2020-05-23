using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtPricingPlan
{
    public class YachtPricingPlanSearchModel : SearchModel
    {
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        public DateTime? EffectiveEndDateFrom { get; set; }
        public DateTime? EffectiveEndDateTo { get; set; }
        public int YachtFID { get; set; }
        public int PricingPlanFId { get; set; }


        
    }
}
