using System.Linq;

namespace AQBooking.Core.Paging
{
    public static class PageListExtensions
    {
        public static PageList<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new PageList<T>(source, pageIndex, pageSize);
        }
    }
}
