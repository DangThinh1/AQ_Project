using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtPricingPlansBase
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int YachtFid { get; set; }
        public int PricingCategoryFid { get; set; }
        public string PricingCategoryResKey { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsRecurring { get; set; }
        public string PlanName { get; set; }
        public bool BasedPortLocation { get; set; }
        public long YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}