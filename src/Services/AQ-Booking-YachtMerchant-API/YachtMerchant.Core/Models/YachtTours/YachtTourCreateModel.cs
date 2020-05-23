using System;

namespace YachtMerchant.Core.Models.YachtTours
{
    public class YachtTourCreateModel
    {
        public int CityFid { get; set; }
        public int CountryFid { get; set; }
        public int LocationFid { get; set; }
        public int MerchantFid { get; set; }
        public int TourCategoryFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public TimeSpan DepartTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public int? TourDurationUnitTypeFid { get; set; }
        public double? TourDurationValue { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string TourName { get; set; }
        public string Remark { get; set; }

        //public string LocationName { get; set; }
        //public string Country { get; set; }
        //public string City { get; set; }
        public string TourCategoryResKey { get; set; }
        public string TourDurationUnitResKey { get; set; }
    }
}
