using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantProductPricings
    {
        public virtual YachtMerchantProductInventories Product { get; set; }
    }
}
