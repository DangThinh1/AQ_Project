using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantProductVendors
    {
        public virtual List<YachtMerchantProductSuppliers> ProductSuppliers { get; set; }

        public YachtMerchantProductVendors()
        {
            ProductSuppliers = new List<YachtMerchantProductSuppliers>();
        }
    }
}
