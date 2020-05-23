using AQBooking.Core.Paging;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelInventoryAttributeValueSearchModel : SearchModel
    {
        public int? AttributeCategoryFid { get; set; }
        public long InventoryFid { get; set; }
        public int AttributeFid { get; set; }

        public HotelInventoryAttributeValueSearchModel() : base()
        {
            AttributeCategoryFid = null;
            InventoryFid = 0;
            AttributeFid = 0;
        }
    }
}
