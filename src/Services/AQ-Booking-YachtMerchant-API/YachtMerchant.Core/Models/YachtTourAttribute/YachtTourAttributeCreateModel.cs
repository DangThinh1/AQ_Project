using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourAttribute
{
    public class YachtTourAttributeCreateModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeName { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Value { get; set; }
        public int YachtTourId { get; set; }
    }
}
