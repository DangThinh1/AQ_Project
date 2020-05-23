using System;
using System.Collections.Generic;
using System.Text;

namespace AQS.BookingMVC.Infrastructure.AQPagination
{
    public interface IPaginatedService
    {
        PaginatedModel GetMetaData(int totalItems, int pageIndex, int pageSize);
    }

    public class PaginatedService : IPaginatedService
    {
        private const int NumberOfNode = 5;

        private List<Page> _pages;

        private readonly IServicesPrevPage _prevPageService;
        private readonly IServicesNextPage _nextPageService;
        private readonly IServicesNode _nodeService;
        private readonly ILastPageInCollectionService _lastPageInCollectionService;

        public PaginatedService()
        {
            _prevPageService = new ServicesPrevPage();
            _nextPageService = new ServicesNextPage();
            _nodeService = new ServicesNode();
            _lastPageInCollectionService = new LastPageInCollectionService();
        }

        public PaginatedModel GetMetaData(int totalItems, int pageIndex, int pageSize)
        {
            var lastPage = _lastPageInCollectionService.GetLastPage(totalItems, pageSize);

            // Cover > out of range exceptions
            if (lastPage < pageIndex)
            {
                pageIndex = lastPage;
            }

            // Cover < out of range exceptions
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (totalItems == 0)
            {
                return GetCollectionSizeZeroModel();
            }

            _pages = _nodeService.BuildPageNodes(totalItems, pageIndex, pageSize, NumberOfNode);
            return new PaginatedModel
            {
                PrevPage = _prevPageService.BuildPrevPage(_pages, totalItems, pageIndex, pageSize),
                Pages = _pages,
                NextPage = _nextPageService.BuildNextPage(_pages, totalItems, pageIndex, pageSize, NumberOfNode)
            };
        }

        private static PaginatedModel GetCollectionSizeZeroModel()
        {
            return new PaginatedModel
            {
                PrevPage = new PrevPage
                {
                    Display = false
                },
                Pages = new List<Page>(),
                NextPage = new NextPage
                {
                    Display = false
                }
            };
        }
    }
}
