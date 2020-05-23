using System;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValuesUpdateRangeModel
    {
        public int YachtFid { get; set; }
        public List<int> ListAttributeId { get; set; }
        public List<int> ListAttributeValue { get; set; }
        public int AttributeCategoryFid { get; set; }
    }
}
