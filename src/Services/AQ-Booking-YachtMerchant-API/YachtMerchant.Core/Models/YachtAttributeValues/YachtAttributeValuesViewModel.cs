using System;

namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValuesViewModel
    {
        public int Id { get; set; }
        public int YachFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeValue { get; set; }
        public bool BasedAffective { get; set; }
        public DateTime? EffectiveDate { get; set; }    
        public string AttributeName { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }    
    }
}
