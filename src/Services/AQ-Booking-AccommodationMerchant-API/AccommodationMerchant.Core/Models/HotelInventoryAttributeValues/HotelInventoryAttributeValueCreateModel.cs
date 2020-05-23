using System;
using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelAttributeValues
{
    public class HotelInventoryAttributeValueCreateModel
    {
        [Range(1, long.MaxValue, ErrorMessage = "The field InventoryFid is required.")]
        [Required]
        public long InventoryFid { get; set; }
        [Required]
        public string AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        [Required]
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class HotelInventoryAttributeValueCreateModels
    {
        [Range(1, long.MaxValue, ErrorMessage = "The field InventoryFid is required.")]
        [Required]
        public long InventoryFid { get; set; }
        [Required]
        public int AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        [Required]
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}