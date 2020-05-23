using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelAttributeValueSearchModel: SearchModel
    {
        public int? AttributeCategoryFid { get; set; }
        public int HotelFid { get; set; }
        public int AttributeFid { get; set; }

        public HotelAttributeValueSearchModel() : base()
        {
            AttributeCategoryFid = null;
            HotelFid = 0;
            AttributeFid = 0;
        }
    }
}
