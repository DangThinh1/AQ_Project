using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductInventories
{
    public class YachtMerchantProductPricingViewModel
    {
      
        public string ProductName { get; set; }
        public int ProductFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public double Price { get; set; }
        public List<ProductPricingViewDetailModel> ListPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
    }
    public class ProductPricingViewDetailModel
    {
        public long Id { get; set; }
        public int ProductFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
    }
}
