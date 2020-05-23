using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtTourCountersBase
    {
        public int YachtTourId { get; set; }
        public string YachtTourUniqueId { get; set; }
        public int TotalViews { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSuccessBookings { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}