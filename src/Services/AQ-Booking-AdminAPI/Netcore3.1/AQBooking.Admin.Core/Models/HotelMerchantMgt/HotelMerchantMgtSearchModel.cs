using AQBooking.Admin.Core.Paging;
using System;

namespace AQBooking.Admin.Core.Models.HotelMerchantMgt
{
    public class HotelMerchantMgtSearchModel : PagableModel
    {
        public string AqadminUserFid { get; set; }
        public int MerchantFid { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
