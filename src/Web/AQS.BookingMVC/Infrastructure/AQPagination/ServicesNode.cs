using System;
using System.Collections.Generic;
using System.Text;

namespace AQS.BookingMVC.Infrastructure.AQPagination
{
    public interface IServicesNode
    {
        List<Page> BuildPageNodes(int totalItems, int pageIndex, int pageSize, int numberOfNode);
    }

    public class ServicesNode : IServicesNode
    {
        private readonly ILastPageInCollectionService _lastPageInCollectionService;

        public ServicesNode()
        {
            _lastPageInCollectionService = new LastPageInCollectionService();
        }

        public List<Page> BuildPageNodes(int totalItems, int pageIndex, int pageSize, int numberOfNode)
        {
            var lastPage = _lastPageInCollectionService.GetLastPage(totalItems, pageSize);
            
            if (numberOfNode * pageSize > totalItems)
            {
                return BuildPartialList(totalItems, pageIndex, pageSize);
            }
            
            if (pageIndex < numberOfNode - 2)
            {
                return BuildStartList(pageIndex);
            }
            
            if (pageIndex > lastPage - (numberOfNode - 2))
            {
                return BuildEndList(totalItems, pageIndex, pageSize, numberOfNode);
            }

            // We are at an in between collection of node in paginated list
            return BuildFullList(pageIndex);
        }

        private static List<Page> BuildFullList(int pageIndex)
        {
            var pages = new List<Page>
            {
                BuildNode(pageIndex - 2),
                BuildNode(pageIndex - 1),
                BuildNode(pageIndex),
                BuildNode(pageIndex + 1),
                BuildNode(pageIndex + 2)
            };

            pages[2].IsCurrent = true;

            return pages;
        }

        private List<Page> BuildPartialList(int totalItems, int pageIndex, int pageSize)
        {
            var pages = new List<Page>();
            for (var i = 0; i < _lastPageInCollectionService.GetLastPage(totalItems, pageSize); i++)
            {
                pages.Add(BuildNode(i + 1));
            }

            pages[pageIndex - 1].IsCurrent = true;

            return pages;
        }

        private static List<Page> BuildStartList(int pageIndex)
        {
            var pages = new List<Page>
            {
                BuildNode(1),
                BuildNode(2),
                BuildNode(3),
                BuildNode(4),
                BuildNode(5)
            };

            pages[pageIndex - 1].IsCurrent = true;

            return pages;
        }

        private List<Page> BuildEndList(int totalItems, int pageIndex, int pageSize, int numberOfNode)
        {
            var lastPage = _lastPageInCollectionService.GetLastPage(totalItems, pageSize);

            var pages = new List<Page>
            {
                BuildNode(lastPage - 4),
                BuildNode(lastPage - 3),
                BuildNode(lastPage - 2),
                BuildNode(lastPage - 1),
                BuildNode(lastPage)
            };

            var unshiftedIndex = lastPage - pageIndex;
            pages[numberOfNode - 1 - unshiftedIndex].IsCurrent = true;

            return pages;
        }

        private static Page BuildNode(int pageNumber)
        {
            return new Page
            {
                IsCurrent = false,
                PageNumber = pageNumber
            };
        }
    }
}
