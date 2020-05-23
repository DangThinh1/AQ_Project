using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformationDetail
{
    public class YachtInformationDetailCreateModel
    {
        public int InformationFid { get; set; }
        public int LanguageFid { get; set; }
        public string ResourceKey { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
