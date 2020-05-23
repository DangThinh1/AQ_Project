namespace AQDiningPortal.Core.Models.Restaurants
{
    public class RestaurantViewModel
    {
        //public int Id { get; set; }
        //public string UniqueId { get; set; }
        //public int MerchantFid { get; set; }
        public string RestaurantName { get; set; }
        public string HseNo { get; set; }
        public string StreetName { get; set; }
        public string BuildingName { get; set; }
        public string ZoneDistrict { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double? StartingPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool ServingVenue { get; set; }
        public bool ServingDining { get; set; }



    }
}
