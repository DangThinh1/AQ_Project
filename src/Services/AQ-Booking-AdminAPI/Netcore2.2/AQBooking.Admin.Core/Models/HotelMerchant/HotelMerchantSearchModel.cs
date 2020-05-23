using AQBooking.Admin.Core.Paging;

namespace AQBooking.Admin.Core.Models.HotelMerchant
{
    public class HotelMerchantSearchModel : PagableModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
