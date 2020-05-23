using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.RestaurantVenue
{
    public class RestaurantVenueViewModel
    {
        public RestaurantVenueViewModel()
        {
            venueInfo = new List<RestaurantVenueInfoViewModel>();
            venuePricing = new List<RestaurantVenuePricingViewModel>();
        }
        public List<RestaurantVenueInfoViewModel> venueInfo { get; set; }
        public List<RestaurantVenuePricingViewModel> venuePricing { get; set; }
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public string DefaultName { get; set; }
        public int Capacity { get; set; }
        public int SeatedCapacity { get; set; }
        public int StandingCapacity { get; set; }
        public double SizeAreaMeter { get; set; }
        public double SizeAreaSqft { get; set; }
        public bool ActiveForOperation { get; set; }
        public bool Deleted { get; set; }
        public double? OrderBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
