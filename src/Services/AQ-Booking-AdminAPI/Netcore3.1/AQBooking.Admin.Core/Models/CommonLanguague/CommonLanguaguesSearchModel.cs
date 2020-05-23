using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.CommonLanguague
{
    public class CommonLanguaguesSearchModel : PagableModel
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
    }
}
