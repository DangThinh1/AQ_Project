using System;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Databases.YachtEntities
{
    public partial class YachtAttributeValues
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeValue { get; set; }
        public bool BasedAffective { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}