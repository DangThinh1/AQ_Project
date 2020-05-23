using System;

namespace YachtMerchant.Core.Models.YachtTourPricings
{
    public partial class YachtTourPricingCreateModel
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int TourPricingTypeFid { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public short MinimumPaxToGo { get; set; }
        public double TourFee { get; set; }
        public string Remark { get; set; }
    }
} 