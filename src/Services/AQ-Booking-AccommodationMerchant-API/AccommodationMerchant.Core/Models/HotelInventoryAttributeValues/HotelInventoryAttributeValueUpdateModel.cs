using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelInventoryAttributeValueUpdateModel
    {
        public long InventoryFid { get; set; }
        public List<int> AttributeList { get; set; }
        public int AttributeCategoryFid { get; set; }
    }
}