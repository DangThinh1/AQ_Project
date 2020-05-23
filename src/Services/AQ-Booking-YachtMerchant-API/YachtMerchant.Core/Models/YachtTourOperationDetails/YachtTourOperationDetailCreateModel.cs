using System;

namespace YachtMerchant.Core.Models.YachtTourOperationDetails
{
    public class YachtTourOperationDetailCreateModel
    {
        public string TourFid { get; set; }
        public string YachtFid { get; set; }
        public string MerchantFid { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int MaximumPassenger { get; set; }
        public bool OverridedTime { get; set; }
        public TimeSpan DepartTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Remark { get; set; }
    }
}
