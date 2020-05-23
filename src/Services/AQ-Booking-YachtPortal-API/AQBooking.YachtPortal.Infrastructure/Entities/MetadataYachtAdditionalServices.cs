using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtAdditionalServices
    {
        public virtual List<YachtAdditionalServiceControls> AdditionalServiceControls { get; set; }
        public virtual List<YachtAdditionalServiceDetails> AdditionalServiceDetails { get; set; }

        public YachtAdditionalServices()
        {
            AdditionalServiceControls = new List<YachtAdditionalServiceControls>();
            AdditionalServiceDetails = new List<YachtAdditionalServiceDetails>();
        }
    }
}
