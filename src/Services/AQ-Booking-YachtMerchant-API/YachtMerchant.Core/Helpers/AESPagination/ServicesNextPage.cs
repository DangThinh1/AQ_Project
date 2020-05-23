using System.Collections.Generic;
using System.Linq;

namespace YachtMerchant.Core.Helpers.AESPagination
{
    public interface IServicesNextPage
    {
        NextPage BuildNextPage(IEnumerable<Page> pages, int totalItems, int pageIndex, int pageSize, int numberOfPage);
    }

    public class ServicesNextPage : IServicesNextPage
    {
        private readonly ILastPageInCollectionService _lastPageCollectionService;

        public ServicesNextPage()
        {
            _lastPageCollectionService = new LastPageInCollectionService();
        }

        public NextPage BuildNextPage(IEnumerable<Page> pages, int totalItems, int pageIndex, int pageSize, int numberOfPage)
        {
            var display = DisplayNextPage(totalItems, pageIndex, pageSize);
            return new NextPage
            {
                Display = display,
                PageNumber = pageIndex == totalItems ? pageIndex : GetPageNumber(display, pages, numberOfPage)
            };
        }

        private static int GetPageNumber(bool display, IEnumerable<Page> pages, int numberOfPage)
        {
            return display ? pages.First(x => x.IsCurrent).PageNumber + 1 : numberOfPage + 1;
        }

        private bool DisplayNextPage(int totalItems, int pageIndex, int pageSize)
        {
            return pageIndex < _lastPageCollectionService.GetLastPage(totalItems, pageSize);
        }
    }
}
