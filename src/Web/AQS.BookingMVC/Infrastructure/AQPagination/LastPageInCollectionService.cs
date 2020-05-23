using System;
using System.Collections.Generic;
using System.Text;

namespace AQS.BookingMVC.Infrastructure.AQPagination
{
    public interface ILastPageInCollectionService
    {
        int GetLastPage(int totalItems, int pageSize);
    }

    public class LastPageInCollectionService : ILastPageInCollectionService
    {
        public int GetLastPage(int totalItems, int pageSize)
        {
            var lastPage = (double)totalItems / pageSize;

            if (lastPage % 1 == 0)
                return (int)lastPage;
            return (int)lastPage + 1;
        }
    }
}
