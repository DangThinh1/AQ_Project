using System.Linq;

namespace AQBooking.Admin.Core.Paging
{
    public static class PageListExtensions
    {
        public static PagedList<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new PagedList<T>(source, pageIndex, pageSize);
        }
    }
}
