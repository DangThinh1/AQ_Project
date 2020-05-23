using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCounters
{
    public class YachtTourCounterViewModel
    {
        public string YachtTourId { get; set; }
        public string TourName { get; set; }
        public string YachtTourUniqueId { get; set; }
        public int TotalViews { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSuccessBookings { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}
