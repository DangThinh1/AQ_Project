using System;
using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.HotelInventories
{
    public class HotelInventorySearchModel : PagableModel
    {
        public string HotelFid { get; set; }
        public string RoomName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
} 