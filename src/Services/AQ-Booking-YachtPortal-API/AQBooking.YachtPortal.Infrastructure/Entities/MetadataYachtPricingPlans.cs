using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtPricingPlans
    {
        public virtual List<YachtPricingPlanDetails> Details { get; set; }
        public virtual List<YachtPricingPlanInformations> Informations { get; set; }
        public virtual YachtPorts Port { get; set; }
        public virtual Yachts Yacht { get; set; }

        public YachtPricingPlans()
        {
            Details = new List<YachtPricingPlanDetails>();
            Informations = new List<YachtPricingPlanInformations>();
        }
    }
}
