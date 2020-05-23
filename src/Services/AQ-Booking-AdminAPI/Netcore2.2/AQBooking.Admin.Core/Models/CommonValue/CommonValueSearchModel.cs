using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.CommonValue
{
    public class CommonValueSearchModel : PagableModel
    {
        public string ValueGroup { get; set; }
        public int? ValueInt { get; set; }
        public string ValueString { get; set; }
        public double? ValueDouble { get; set; }
        public string Text { get; set; }
    }
}
