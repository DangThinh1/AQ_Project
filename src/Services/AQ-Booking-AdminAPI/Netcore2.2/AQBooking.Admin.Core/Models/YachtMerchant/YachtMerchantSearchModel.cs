using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchant
{
    public class YachtMerchantSearchModel : SearchModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
