using System;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Databases.YachtEntities
{
    public partial class YachtAttributes
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeName { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Remarks { get; set; }
        public bool? IsDefault { get; set; }
        public double? OrderBy { get; set; }
        public bool? Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}