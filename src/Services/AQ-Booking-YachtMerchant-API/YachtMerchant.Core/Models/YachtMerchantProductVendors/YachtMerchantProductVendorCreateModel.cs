using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductVendors
{
   public class YachtMerchantProductVendorCreateModel
    {
        public int MerchantFid { get; set; }
        public int ProductCategoryFid { get; set; }
        public string ProductCategoryResKey { get; set; }
        public string Name { get; set; }
        public int VendorTypeFid { get; set; }
        public string VendorTypeResKey { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
}
