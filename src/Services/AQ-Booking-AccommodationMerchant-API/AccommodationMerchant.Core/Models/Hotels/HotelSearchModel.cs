using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.Hotels
{
    public class HotelSearchModel : PagableModel
    {
        public string MerchantFid { get; set; }
        public string HotelName { get; set; }
        public bool? ActiveForOperation { get; set; }
    }
}