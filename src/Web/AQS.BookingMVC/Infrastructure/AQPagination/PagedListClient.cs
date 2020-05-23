using AQS.BookingMVC.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.AQPagination
{
    public class PagedListClient<T>
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
        public int PageIndex => this.PageNumber > 0 ? this.PageNumber - 1 : 0;

        public PaginatedModel PaginatedModel
        {
            get
            {
                return DependencyInjectionHelper.GetService<IPaginatedService>().GetMetaData(TotalItems, PageIndex, PageSize);
            }
        }
    }
}
