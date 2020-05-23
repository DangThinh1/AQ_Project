using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtAdditionalServiceDetails
    {
        public virtual YachtAdditionalServices AdditionalService { get; set; }
        public virtual YachtMerchantProductInventories Product { get; set; }
    }
}
