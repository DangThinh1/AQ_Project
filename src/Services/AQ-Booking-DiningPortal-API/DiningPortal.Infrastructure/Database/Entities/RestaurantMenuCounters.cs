using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantMenuCounters
    {
        public int Id { get; set; }
        public int MenuFid { get; set; }
        public string MenuUniqueId { get; set; }
        public int TotalReservations { get; set; }
        public int TotalSuccessReservations { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}
