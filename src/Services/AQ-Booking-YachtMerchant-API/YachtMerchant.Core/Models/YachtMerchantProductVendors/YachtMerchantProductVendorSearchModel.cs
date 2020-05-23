using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductVendors
{
    public class YachtMerchantProductVendorSearchModel: SearchModel
    {
        public int MerchantId { get; set; }
        public string VendorName { get; set; }
        public int CategoryId { get; set; }
        public int VendorTypeId { get; set; }
    }
}
