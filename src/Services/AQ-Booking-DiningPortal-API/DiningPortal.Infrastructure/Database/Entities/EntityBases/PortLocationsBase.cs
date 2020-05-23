using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class PortLocationsBase
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZoneDistrict { get; set; }
        public string PickupPointName { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}