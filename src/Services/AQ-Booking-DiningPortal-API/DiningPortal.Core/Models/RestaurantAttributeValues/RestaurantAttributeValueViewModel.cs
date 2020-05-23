using System;

namespace AQDiningPortal.Core.Models.RestaurantAttributeValues
{
    public class RestaurantAttributeValueViewModel
    {
        //public int Id { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeValue { get; set; }
        public string ResourceKey { get; set; }
    }
}
