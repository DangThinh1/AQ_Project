using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtTours
{
    public class YachtTourViewModel
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public long TourImageFileFid { get; set; }
        public int YachtFid { get; set; }
        public string YachtModel { get; set; }
        public string Duration { get; set; }
        public double TourDurationValue { get; set; }
        public int TourDurationUnitTypeFid { get; set; }
        public string TourDurationUnitResKey { get; set; }
        public double TourDurationByDay { get; set; }
        public int MerchantFid { get; set; }
        public int RatingStar { get; set; }
        public double TourFee { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int TourPricingTypeFid { get; set; }
        public string TourPricingResKey { get; set; }
        public int TotalView { get; set; }
        public int TotalSuccessBooking { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
