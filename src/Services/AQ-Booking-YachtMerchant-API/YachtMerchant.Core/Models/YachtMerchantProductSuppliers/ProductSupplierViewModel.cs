using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductSuppliers
{
    public class ProductSupplierViewModel
    {
        public int ProductFid { get; set; }
        public int VendorFid { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
