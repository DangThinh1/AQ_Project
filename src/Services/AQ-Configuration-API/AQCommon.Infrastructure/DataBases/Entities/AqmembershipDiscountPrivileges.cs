using System;
using System.Collections.Generic;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class AqmembershipDiscountPrivileges
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int RoleFid { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
