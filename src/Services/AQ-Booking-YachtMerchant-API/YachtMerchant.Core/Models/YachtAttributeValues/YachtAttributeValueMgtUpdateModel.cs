﻿
namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValueMgtUpdateModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public bool Check { get; set; }
    }
}
