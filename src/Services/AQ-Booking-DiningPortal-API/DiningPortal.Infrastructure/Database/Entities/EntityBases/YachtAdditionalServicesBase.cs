using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtAdditionalServicesBase
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int MerchantFid { get; set; }
        public int AdditonalServiceTypeFid { get; set; }
        public string AdditonalServiceTypeResKey { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}