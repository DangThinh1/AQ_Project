using System;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public partial class YachtAdditionalServiceDetailCreateModel
    {
        public int AdditionalServiceFid { get; set; }
        public int ProductFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }

    }
}
