using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtPricingPlan
{
    public class YachtPricingPlanInformationCreateModel
    {
        public long Id { get; set; }
        public int PricingPlanFid { get; set; }
        public int LanguageFid { get; set; }
        public string PackageInfo { get; set; }        
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LanguagueName { get; set; }



    }
}
