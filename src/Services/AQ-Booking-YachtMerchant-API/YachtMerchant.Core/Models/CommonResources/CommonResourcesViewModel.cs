using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.CommonResources
{
    public class CommonResourcesViewModel
    {
        public int ResourceId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public int? LanguageFid { get; set; }
        public string TypeOfResource { get; set; }
    }
}
