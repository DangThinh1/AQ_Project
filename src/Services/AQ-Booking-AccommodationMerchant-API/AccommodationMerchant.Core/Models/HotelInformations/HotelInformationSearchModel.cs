using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.HotelInformations
{
    public class HotelInformationSearchModel : SearchModel
    {
        //Decrypt values
        public string HotelFid { get; set; }

        //Normal values
        public string Title { get; set; }
        public bool? IsActivated { get; set; }
        public string ActivatedDate { get; set; }
    }
}