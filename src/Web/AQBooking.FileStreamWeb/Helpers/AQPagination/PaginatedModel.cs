using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStreamWeb.Helpers.AQPagination
{
    public class PaginatedModel
    {
        public PaginatedModel()
        {
            List<Page> Pages = new List<Page>();
        }

        public List<Page> Pages { get; set; }
        public PrevPage PrevPage { get; set; }
        public NextPage NextPage { get; set; }
    }

    public class PrevPage
    {
        public bool Display { get; set; }
        public int PageNumber { get; set; }
    }

    public class Page
    {
        public int PageNumber { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class NextPage
    {
        public bool Display { get; set; }
        public int PageNumber { get; set; }
    }
}
