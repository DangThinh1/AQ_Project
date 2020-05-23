using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public class YachtAdditionalServiceControlViewModel
    {
        public int AdditionalServiceFid { get; set; }
        public string AdditionalServiceName { get; set; }
        public int YachtFid { get; set; }
        public string YachtName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
        public List<YachtAdditionalServiceControlModel> ListServiceControl { get; set; }
    }

    public class YachtAdditionalServiceControlModel
    {
        public int AdditionalServiceFid { get; set; }
        public int YachtFid { get; set; }
        public string YachtName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
