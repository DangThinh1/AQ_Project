using System;

namespace YachtMerchant.Core.Models.YachtTourAttributeValues
{
    public class YachtTourAttributeValueCreateModel
    {
        public string YachtTourFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeValue { get; set; }
    }
}
