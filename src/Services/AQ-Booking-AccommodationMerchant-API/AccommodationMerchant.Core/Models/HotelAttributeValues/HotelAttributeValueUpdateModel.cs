using System;
using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelAttributeValueUpdateModel
    {
        public int HotelFid { get; set; }
        public List<int> AttributeList { get; set; }
        public int AttributeCategoryFid { get; set; }
    }
}