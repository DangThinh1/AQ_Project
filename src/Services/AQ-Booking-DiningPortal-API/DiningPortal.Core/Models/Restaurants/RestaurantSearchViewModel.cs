using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.Restaurants
{
    public class RestaurantSearchViewModel
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }
        public int MerchantFid { get; set; }
        public string RestaurantName { get; set; }
        public bool IsBusinessDay { get; set; }
        public string CuisineName { get; set; }
        public string StreetName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZoneDistrict { get; set; }
        public int? FileStreamId { get; set; }
        public List<int>AttributeCheckedId { get; set; }
        public double? StartingPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool ServingVenue { get; set; }
        public bool ServingDining { get; set; }
    }
}
