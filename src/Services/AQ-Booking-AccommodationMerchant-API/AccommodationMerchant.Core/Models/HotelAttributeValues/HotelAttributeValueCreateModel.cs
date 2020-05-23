using System;
using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelAttributeValueCreateModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "The field HotelFid is required.")]
        public int HotelFid { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The field AttributeFid is required.")]
        public string AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class HotelAttributeValueCreateModels
    {
        [Range(1, int.MaxValue, ErrorMessage = "The field HotelFid is required.")]
        public int HotelFid { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The field AttributeFid is required.")]
        public int AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
} 