using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.EVisaMerchant
{
    public class EVisaMerchantSearchModel : SearchModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
