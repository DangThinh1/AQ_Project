using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtAttribute
{
    public class YachtAttributeSearchModel : PagableModel
    {
        public int? AttributeCategoryID { get; set; }
        public string AttributeName { get; set; }
    }
}
