using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class Restaurants
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int MerchantFid { get; set; }
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
        public bool ActiveForOperation { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool ServingVenue { get; set; }
        public bool ServingDining { get; set; }

        //Relationship Properties
        public virtual RestaurantMerchants Merchant { get; set; }

        public virtual ICollection<RestaurantAttributeValues>AttributeValues { get; set; }
        public virtual ICollection<RestaurantInformations> Informations { get; set; }
        public virtual ICollection<RestaurantOtherInformations> OtherInformations { get; set; }
        public virtual ICollection<RestaurantBusinessDays> BusinessDays { get; set; }
        public virtual ICollection<RestaurantNonBusinessDays> NonBusinessDays { get; set; }
        public virtual ICollection<RestaurantReservationFees> ReservationFees { get; set; }
        public virtual ICollection<RestaurantFileStreams> FileStreams { get; set; }
        public virtual ICollection<RestaurantMenus> Menus { get; set; }
        public virtual RestaurantCounters Counter { get; set; }

        public Restaurants()
        {
            AttributeValues = new HashSet<RestaurantAttributeValues>();
            Informations = new HashSet<RestaurantInformations>();
            OtherInformations = new HashSet<RestaurantOtherInformations>();
            ReservationFees = new HashSet<RestaurantReservationFees>();
            FileStreams = new HashSet<RestaurantFileStreams>();
            Menus = new HashSet<RestaurantMenus>();
            NonBusinessDays = new HashSet<RestaurantNonBusinessDays>();
            BusinessDays = new HashSet<RestaurantBusinessDays>();
            Counter = new RestaurantCounters();
        }
    }
}
