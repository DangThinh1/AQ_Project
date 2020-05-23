using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantProductInventories
    {
        public virtual List<YachtAdditionalServiceDetails> AdditionalServiceDetails { get; set; }
        public virtual List<YachtMerchantProductPricings> ProductPricings { get; set; }
        public virtual List<YachtMerchantProductSuppliers> ProductSuppliers { get; set; }

        public YachtMerchantProductInventories()
        {
            AdditionalServiceDetails = new List<YachtAdditionalServiceDetails>();
            ProductPricings = new List<YachtMerchantProductPricings>();
            ProductSuppliers = new List<YachtMerchantProductSuppliers>();
        }
    }
}
