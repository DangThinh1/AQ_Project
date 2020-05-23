using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.CommonLanguague
{
    public class CommonLanguaguesUpdateModel
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public string Remarks { get; set; }
        public string ResourceKey { get; set; }
    }
}
