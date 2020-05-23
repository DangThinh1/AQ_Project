using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantOpeningHours
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public string WorkingTimeName { get; set; }
        public string WorkingStartTime { get; set; }
        public string WorkingEndTime { get; set; }
        public string WorkingTimeDisplay { get; set; }
        public bool IsActivated { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
