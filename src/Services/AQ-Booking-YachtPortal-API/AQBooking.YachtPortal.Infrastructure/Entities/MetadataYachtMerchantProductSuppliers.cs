using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantProductSuppliers
    {
        public virtual YachtMerchantProductInventories Product { get; set; }
        public virtual YachtMerchantProductVendors Vendor { get; set; }
    }
}
