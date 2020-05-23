using System;
using System.Collections.Generic;
using System.Text;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class ZoneDistricts
    {
        public int ID { get; set; }
        public int CityCode { get; set; }
        public string ZoneDistrictName { get; set; }
    }
}
