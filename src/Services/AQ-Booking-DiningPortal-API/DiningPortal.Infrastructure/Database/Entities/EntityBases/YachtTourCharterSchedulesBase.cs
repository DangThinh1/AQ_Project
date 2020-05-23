using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtTourCharterSchedulesBase
    {
        public long Id { get; set; }
        public long TourCharterFid { get; set; }
        public int YachtFid { get; set; }
        public Guid UserFid { get; set; }
        public int RoleFid { get; set; }
        public string RoleResKey { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public Guid AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool Deleted { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}