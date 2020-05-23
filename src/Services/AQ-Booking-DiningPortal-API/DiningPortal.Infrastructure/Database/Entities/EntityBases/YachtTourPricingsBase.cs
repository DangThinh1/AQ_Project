using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtTourPricingsBase
    {
        public long Id { get; set; }
        public int TourFid { get; set; }
        public int YachtFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int TourPricingTypeFid { get; set; }
        public string TourPricingTypeResKey { get; set; }
        public short MinimumPaxToGo { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double TourFee { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}