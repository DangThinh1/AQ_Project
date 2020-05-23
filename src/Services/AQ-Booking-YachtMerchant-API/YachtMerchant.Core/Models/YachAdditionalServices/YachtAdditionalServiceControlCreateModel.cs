using System;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public partial class YachtAdditionalServiceControlCreateModel
    {
        public int YachtFid { get; set; }
        public int AdditionalServiceFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
