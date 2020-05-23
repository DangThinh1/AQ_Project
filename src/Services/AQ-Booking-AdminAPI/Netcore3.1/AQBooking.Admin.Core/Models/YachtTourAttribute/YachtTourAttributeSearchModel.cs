using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtTourAttribute
{
    public class YachtTourAttributeSearchModel : PagableModel
    {
        public int? AttributeCategoryID { get; set; }
        public string AttributeName { get; set; }
    }
}
