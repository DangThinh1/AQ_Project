using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformation
{
    public class YachtInformationSupportModel
    {
        public long Id { get; set; }
        public int LanguageFid { get; set; }
        public string LanguageName { get; set; }
        public string ResourceKey { get; set; }
        public bool Supported { get; set; }
        public int? FileStreamFid { get; set; }
        public int InfoFid { get; set; }
    }
}
