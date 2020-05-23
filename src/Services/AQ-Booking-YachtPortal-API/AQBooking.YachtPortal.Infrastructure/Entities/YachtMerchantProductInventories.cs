using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantProductInventories
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
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
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}