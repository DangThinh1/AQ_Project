using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQDiningPortal.Core.Models.PagingPortal
{
    public class PageListPortal<T>:PagedListBase <T> where T:class
    {
        public PageListPortal():base()
        {

        }
        public PageListPortal(List<T> source):base (source)
        {

        }

        public PageListPortal(IQueryable<T> source, int pageNumber, int pageSize): base(source, pageNumber, pageSize)
        {

        }

        public PageListPortal(List<T> source, int pageIndex, int pageSize, int totalItems):base(source, pageIndex, pageSize, totalItems)
        {

        }
    } 
}
