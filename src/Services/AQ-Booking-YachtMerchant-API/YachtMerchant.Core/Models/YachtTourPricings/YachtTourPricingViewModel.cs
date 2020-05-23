using System;

namespace YachtMerchant.Core.Models.YachtTourPricings
{
    public partial class YachtTourPricingViewModel
    {
        public long Id { get; set; }
        public string TourFid { get; set; }
        public string YachtFid { get; set; }
        public string YachtName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int TourPricingTypeFid { get; set; }
        public string TourPricingTypeResKey { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public short MinimumPaxToGo { get; set; }
        public double TourFee { get; set; }
        public string Remark { get; set; }
    }
}