using System;

namespace YachtMerchant.Core.Models.YachtTourOperationDetails
{
    public class YachtTourOperationDetailUpdateModel
    {
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