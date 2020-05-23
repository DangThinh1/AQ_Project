using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class CommonLanguages
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public string CssClass { get; set; }
        public string ResourceKey { get; set; }
        public bool Deleted { get; set; }
        public string Remarks { get; set; }
    }
}