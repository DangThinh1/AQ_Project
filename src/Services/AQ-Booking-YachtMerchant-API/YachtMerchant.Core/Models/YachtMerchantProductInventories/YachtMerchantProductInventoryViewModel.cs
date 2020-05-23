using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductInventories
{
    public class YachtMerchantProductInventoryViewModel
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryFid { get; set; }
        public string ProductCategoryResKey { get; set; }
        public string Description { get; set; }
        public int GsttypeFid { get; set; }
        public string GsttypeResKey { get; set; }
        public bool IsActiveForSales { get; set; }
        public bool IsStockControl { get; set; }
        public int PriceTypeFid { get; set; }
        public string PriceTypeResKey { get; set; }
        public int ItemUnitFid { get; set; }
        public string ItemUnitResKey { get; set; }
        public int Quantities { get; set; }
        //public DateTime EffectiveDate { get; set; }
        //public DateTime? EffectiveEndDate { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int VendorFid { get; set; }
        //public DateTime EffectiveDateSupplier { get; set; }
        //public DateTime? EffectiveEndDateSupplier { get; set; }
        public string Remark { get; set; }
        public int Count { get; set; }
    }
}
