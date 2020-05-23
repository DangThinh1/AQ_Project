using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtPricingPlanInformationsBase
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
    }
}