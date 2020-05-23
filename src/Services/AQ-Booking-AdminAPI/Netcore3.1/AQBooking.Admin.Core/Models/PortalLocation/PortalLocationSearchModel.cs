
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PortalLocation
{
    public class PortalLocationSearchModel : PagableModel
    {
        public string PortalUniqueId { get; set; }
        public int PortalId { get; set; }
        public int CityCode { get; set; }
        public int CountryCode { get; set; }
    }
}
