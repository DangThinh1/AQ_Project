using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantBusinessDays
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public int BusinessDay { get; set; }
        public string DayOfWeek { get; set; }
        public bool Deleted { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Restaurants Restaurant { get; set; }
        public virtual List<RestaurantBusinessDayOperations> BusinessDayOperations { get; set; }
    }
}
