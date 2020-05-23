using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourPreview
{
    public class YachtTourDetailModel
    {
        public int TourId { get; set; }
        public int TourInfoFid { get; set; }
        public string TourName { get; set; }
        public int RatingStar { get; set; }
        public int TotalView { get; set; }
        public int TotalSuccessBooking { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public int TourCategoryFid { get; set; }
        public string TourCategoryResKey { get; set; }
    }
}