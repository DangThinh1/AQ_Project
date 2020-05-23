using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtToursBase
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int MerchantFid { get; set; }
        public int TourCategoryFid { get; set; }
        public string TourCategoryResKey { get; set; }
        public string TourName { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int LocationFid { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}