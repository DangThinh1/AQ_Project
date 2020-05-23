using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class CommonResources
    {
        public string ResourceKey { get; set; }
        public int LanguageFid { get; set; }
        public string ResourceValue { get; set; }
        public string TypeOfResource { get; set; }
    }
}