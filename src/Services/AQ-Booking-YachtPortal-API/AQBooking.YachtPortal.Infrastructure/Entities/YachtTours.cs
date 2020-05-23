using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtTours
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int MerchantFid { get; set; }
        public int TourCategoryFid { get; set; }
        public string TourCategoryResKey { get; set; }
        public string TourName { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int LocationFid { get; set; }
        public string LocationName { get; set; }
        public int CountryFid { get; set; }
        public string Country { get; set; }
        public int CityFid { get; set; }
        public string City { get; set; }
        public TimeSpan DepartTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public double TourDurationValue { get; set; }
        public int TourDurationUnitTypeFid { get; set; }
        public string TourDurationUnitResKey { get; set; }
        public double TourDurationByDay { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}