using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.CommonResource
{
    public class CommonResourcesSearchModel : PagableModel
    {
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public int? LanguageFid { get; set; }
        public string TypeOfResource { get; set; }
    }
}
