using AQBooking.Core.Paging;
using System;

namespace AccommodationMerchant.Core.Models.HotelAttributes
{
    public class HotelAttributeSearchModel : SearchModel
    {
        public int? AttributeCategoryFid { get; set; }

        public HotelAttributeSearchModel():base()
        {
            AttributeCategoryFid = null;
        }
    }
}
