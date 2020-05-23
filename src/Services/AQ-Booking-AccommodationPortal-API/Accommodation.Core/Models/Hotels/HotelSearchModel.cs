using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accommodation.Core.Models.Hotels
{
    public class HotelSearchModel : PagableModel
    {
        public string MerchantFid { get; set; }
        public string HotelName { get; set; }
        public bool? ActiveForOperation { get; set; }
    }
} 
