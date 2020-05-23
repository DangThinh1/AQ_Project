using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.PortLocation
{
    public class PortLocationCreateModel
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZoneDistrict { get; set; }
        public string PickupPointName { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
