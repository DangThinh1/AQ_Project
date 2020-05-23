using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtTourOperationDetails
    {
        public int TourFid { get; set; }
        public int YachtFid { get; set; }
        public int MerchantFid { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int MaximumPassenger { get; set; }
        public bool OverridedTime { get; set; }
        public TimeSpan DepartTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}