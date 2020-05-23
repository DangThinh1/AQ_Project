using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantVenues
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public string UniqueId { get; set; }
        public string DefaultName { get; set; }
        public int Capacity { get; set; }
        public int SeatedCapacity { get; set; }
        public int StandingCapacity { get; set; }
        public double SizeAreaMeter { get; set; }
        public double SizeAreaSqft { get; set; }
        public bool ActiveForOperation { get; set; }
        public bool Deleted { get; set; }
        public double? OrderBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
