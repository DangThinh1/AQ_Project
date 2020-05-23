using System;
using System.Collections.Generic;

namespace Accommodation.Infrastructure.Databases.Entities
{
    public partial class HotelInventoryRates
    {
        public long Id { get; set; }
        public long InventoryFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool HasPrice { get; set; }
        public bool Discounted { get; set; }
        public bool WithBreakfast { get; set; }
        public double? Price { get; set; }
        public double? OriginalPrice { get; set; }
        public double? DiscountedValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool Deleted { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}