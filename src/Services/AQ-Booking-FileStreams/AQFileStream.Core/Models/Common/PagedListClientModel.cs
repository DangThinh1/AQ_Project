using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.Common
{
    public class PagedListClientModel<T>
    {
        public int TotalItems { get; set; } 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int NextPageNumber { get; set; }
        public int PreviousPageNumber { get; set; }
    }
}
