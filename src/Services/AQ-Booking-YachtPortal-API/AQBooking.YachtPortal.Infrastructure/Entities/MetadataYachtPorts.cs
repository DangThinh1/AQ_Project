using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtPorts
    {
        public virtual Yachts Yacht { get; set; }
        public virtual PortLocations PortLocation { get; set; }
        public virtual List<YachtPricingPlans> PricingPlans { get; set; }

        public YachtPorts()
        {
            PricingPlans = new List<YachtPricingPlans>();
        }
    }
}
