using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtCounters
{
    public class YachtCounterViewModel
    {
        public int TotalViews { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSuccessBookings { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}
