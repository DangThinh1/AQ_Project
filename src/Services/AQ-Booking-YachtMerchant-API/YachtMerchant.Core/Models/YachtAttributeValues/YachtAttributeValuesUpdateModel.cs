using System;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValuesUpdateModel
    {
        public int YachtFid { get; set; }
        public List<int> AttributeList { get; set; }
        public int AttributeCategoryFid { get; set; }
    }
}