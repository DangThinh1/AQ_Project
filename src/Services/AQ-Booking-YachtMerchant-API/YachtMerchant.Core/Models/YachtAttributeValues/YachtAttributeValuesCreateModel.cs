using System;
using System.ComponentModel.DataAnnotations;

namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValuesCreateModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "The YachtFid field is required.")]
        public int YachtFid { get; set; }
        public string AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class YachtAttributeValuesCreateModels
    {
        public int YachtFid { get; set; }
        public int AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeValue { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}