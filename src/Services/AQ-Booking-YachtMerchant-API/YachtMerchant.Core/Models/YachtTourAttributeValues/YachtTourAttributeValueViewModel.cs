using System;

namespace YachtMerchant.Core.Models.YachtTourAttributeValues
{
    public class YachtTourAttributeValueViewModel
    {
        public long Id { get; set; }
        public string YachtTourFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeValue { get; set; }
        public bool BasedAffective { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}