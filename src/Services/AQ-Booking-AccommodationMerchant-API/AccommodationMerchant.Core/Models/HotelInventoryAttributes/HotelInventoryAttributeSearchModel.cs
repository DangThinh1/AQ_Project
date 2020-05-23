using AQBooking.Core.Paging;
using System;

namespace AccommodationMerchant.Core.Models.HotelAttributes
{
    public class HotelInventoryAttributeSearchModel : SearchModel
    {
        public int? AttributeCategoryFid { get; set; }

        public HotelInventoryAttributeSearchModel():base()
        {
            AttributeCategoryFid = null;
        }
    }
}
