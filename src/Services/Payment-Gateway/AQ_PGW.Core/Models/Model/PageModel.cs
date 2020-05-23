using AQ_PGW.Core.Models.DBTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Models.Model
{
    public class SearchTransactionsModel
    {
        public PageModel<Transactions> ViewAll { get; set; }
        public PageModel<Transactions> ViewCompleted { get; set; }
        public PageModel<Transactions> ViewPaid { get; set; }
        public PageModel<Transactions> ViewPending { get; set; }
        public PageModel<Transactions> ViewNoPaid { get; set; }
    }
    public class PageModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public Pager Pager { get; set; }
    }
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager(int totalItems, int? page, int pageSize = 10)
        {
            try
            {
                // calculate total, start and end pages
                var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
                var currentPage = page != null ? (int)page : 1;
                var startPage = currentPage - 5;
                var endPage = currentPage + 4;
                if (startPage <= 0)
                {
                    endPage -= (startPage - 1);
                    startPage = 1;
                }
                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    if (endPage > 10)
                    {
                        startPage = endPage - 9;
                    }
                }

                TotalItems = totalItems;
                CurrentPage = currentPage;
                PageSize = pageSize;
                TotalPages = totalPages;
                StartPage = startPage;
                EndPage = endPage;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
