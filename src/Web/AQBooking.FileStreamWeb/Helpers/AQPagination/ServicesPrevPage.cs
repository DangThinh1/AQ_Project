using System.Linq;
using System.Collections.Generic;

namespace AQBooking.FileStreamWeb.Helpers.AQPagination
{
    public interface IServicesPrevPage
    {
        PrevPage BuildPrevPage(IEnumerable<Page> pages, int totalItems, int pageIndex, int pageSize);
    }

    public class ServicesPrevPage : IServicesPrevPage
    {
        public PrevPage BuildPrevPage(IEnumerable<Page> pages, int totalItems, int pageIndex, int pageSize)
        {
            var display = DisplayPrevPage(totalItems, pageIndex, pageSize);
            return new PrevPage
            {
                Display = display,
                PageNumber = display ? pages.First(x => x.IsCurrent).PageNumber - 1 : 1
            };
        }

        private static bool DisplayPrevPage(int totalItems, int pageIndex, int pageSize)
        {
            return pageIndex > 1 && totalItems >= pageSize;
        }
    }
}
