using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantReservationTimesheetDetails
    {
        public int Id { get; set; }
        public int TimeFid { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
