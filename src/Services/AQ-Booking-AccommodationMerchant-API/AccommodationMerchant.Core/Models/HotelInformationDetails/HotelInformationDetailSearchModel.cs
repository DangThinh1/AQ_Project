using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.HotelInformationDetails
{
    public class HotelInformationDetailSearchModel : PagableModel
    {
        public string InformationFid { get; set; }
    }
}
