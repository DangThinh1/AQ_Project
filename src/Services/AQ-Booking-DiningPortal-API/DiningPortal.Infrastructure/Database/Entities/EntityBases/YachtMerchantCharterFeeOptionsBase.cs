using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtMerchantCharterFeeOptionsBase
    {
        public long Id { get; set; }
        public int MerchantFid { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
        public int BookingFeeOptionFid { get; set; }
        public string BookingFeeOptionResKey { get; set; }
        public bool IsSchemeBased { get; set; }
        public int SchemeBasedOptionFid { get; set; }
        public string SchemeBasedOptionResKey { get; set; }
        public double Level { get; set; }
        public double BookingFeePercent { get; set; }
        public double MinTarget { get; set; }
        public double MaxTarget { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}