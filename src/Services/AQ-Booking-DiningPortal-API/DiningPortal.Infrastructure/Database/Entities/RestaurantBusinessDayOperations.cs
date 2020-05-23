using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantBusinessDayOperations
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public int RestaurantBusinessDayFid { get; set; }
        public string WorkingStartTime { get; set; }
        public double? WorkingStartTimeValue { get; set; }
        public string WorkingEndTime { get; set; }
        public double? WorkingEndTimeValue { get; set; }
        public string WorkingTimeDisplay { get; set; }
        public bool HaveBreakTime { get; set; }
        public string BreakTimeStart { get; set; }
        public double? BreakTimeStartValue { get; set; }
        public string BreakTimeEnd { get; set; }
        public double? BreakTimeEndValue { get; set; }
        public string BreakTimeDisplay { get; set; }
        public bool IsActivated { get; set; }
        public bool Deleted { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual RestaurantBusinessDays RestaurantBusinessDay { get; set; }
    }
}
