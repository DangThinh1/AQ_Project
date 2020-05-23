using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.HotelMerchantUser
{
    public class HotelMerchantUserSearchModel : PagableModel
    {
        public string UserFid { get; set; }
        public int MerchantFid { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
