using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelInventoryAttributeValueUpdateRangeModel
    {
        public long InventoryFid { get; set; }
        public List<int> ListAttributeId { get; set; }
        public List<string> ListAttributeValue { get; set; }
        public int AttributeCategoryFid { get; set; }
    }
}
