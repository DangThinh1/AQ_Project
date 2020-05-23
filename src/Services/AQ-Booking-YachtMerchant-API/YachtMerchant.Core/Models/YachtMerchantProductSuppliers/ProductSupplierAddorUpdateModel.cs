using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductSuppliers
{
    public class ProductSupplierAddorUpdateModel
    {
        public int ProductFid { get; set; }
        public int VendorFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
