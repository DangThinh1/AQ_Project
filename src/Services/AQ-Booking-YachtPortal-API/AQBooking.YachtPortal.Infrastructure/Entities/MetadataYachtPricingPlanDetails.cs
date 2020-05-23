using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtPricingPlanDetails
    {
        public virtual YachtPricingPlans PricingPlan { get; set; }
    }
}
