using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtCountersBase
    {
        public int YachtId { get; set; }
        public string YachtUniqueId { get; set; }
        public int TotalViews { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSuccessBookings { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}