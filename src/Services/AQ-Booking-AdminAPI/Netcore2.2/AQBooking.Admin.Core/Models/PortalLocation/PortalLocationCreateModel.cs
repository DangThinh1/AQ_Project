using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PortalLocation

{
    public class PortalLocationCreateModel
    {
        public int Id { get; set; }
        public string PortalUniqueId { get; set; }
        public int DomainPortalFID { get; set; }
        public string CountryName { get; set; }
        public int CountryCode { get; set; }
        public string CityName { get; set; }
        public int CityCode { get; set; }
        public string CssClass { get; set; }
        public int FileStreamFID { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
    }
}
