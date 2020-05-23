using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductInventories
{
    public class YachtMerchantProductSupplierViewModel
    {
        public int ProductFid { get; set; }
        public int VendorFid { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
        public List<YachtMerchantProductSupplierDetailModel> ListProductSupplier { get; set; }
    }
    public class YachtMerchantProductSupplierDetailModel
    {
        public int VendorFid { get; set; }
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public int ProductFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }

}
