using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.PortLocations
{
    public class CityPortLocationViewModel
    {
        public long ID { get; set; }
        public string CityName { get; set; }
        public string UniqueId { get; set; }
        public string CountryName { get; set; }
        public string ZoneDistrict { get; set; }
        public string PickupPointName { get; set; }
        
    }
}
