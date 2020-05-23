using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.CommonLanguagues
{
    public class CommonLanguagesResponseModel
    {
        public int Id { get; set; }
        public string UniqueID { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public string ResourceKey { get; set; }
        public bool? Deleted { get; set; }
        public string Remarks { get; set; }
    }
}
