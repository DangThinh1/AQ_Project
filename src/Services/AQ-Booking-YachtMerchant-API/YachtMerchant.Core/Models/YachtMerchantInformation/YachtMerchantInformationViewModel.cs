using System;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtMerchantInformation
{
    public class YachtMerchantInformationViewModel
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public string ResourceKey { get; set; }
        public List<string> LanguagesSupported { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}