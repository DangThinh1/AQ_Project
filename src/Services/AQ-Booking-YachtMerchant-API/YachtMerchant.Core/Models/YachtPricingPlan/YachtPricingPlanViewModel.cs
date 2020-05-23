using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtPricingPlan
{
    public class YachtPricingPlanViewModel : YachtPricingPlanCreateModel
    {
        public List<YachtPricingPlanDetailCreateModel> yachtPricingDetail { get; set; }
    }
}
