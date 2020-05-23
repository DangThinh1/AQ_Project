using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.Common
{
    public class PagableModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortType { get; set; }
        public string SortString
        {
            get
            {
                if (!string.IsNullOrEmpty(SortColumn) && !string.IsNullOrEmpty(SortType))
                    return $"{SortColumn} {SortType}";
                return "";
            }
        }

        public PagableModel()
        {
            PageIndex = PageIndex > 0 ? PageIndex : 1;
            PageSize = PageSize > 0 ? PageSize : 10;
        }
    }
}
