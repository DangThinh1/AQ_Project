using System;
using System.Collections.Generic;
using System.Linq;

namespace AQDiningPortal.Core.Models.PagingPortal
{
    public class PagedListBase<T> where T : class
    {
        public PagedListBase()
        {
            this.TotalItems = 0;
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Data = new List<T>();
        }

        public PagedListBase(IQueryable<T> source, int pageNumber, int pageSize)
        {
            this.TotalItems = source.Count();
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;

            if (source != null && source.Count() > 0)
            {
                this.Data = source
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
                this.Data = source.ToList();
        }

        // Default result output if no data found
        public PagedListBase(List<T> source)
        {
            this.TotalItems = 0;
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Data = source;
        }

        public PagedListBase(List<T> source, int pageIndex, int pageSize, int totalItems)
        {
            this.TotalItems = totalItems;
            this.PageNumber = pageIndex;
            this.PageSize = pageSize;
            this.Data = source;
        }

        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public List<T> Data { get; }
        public int TotalPages =>
                (int)Math.Ceiling(this.TotalItems / (double)this.PageSize);
        public bool HasPreviousPage => this.PageNumber > 1;
        public bool HasNextPage => this.PageNumber < this.TotalPages;
        public int NextPageNumber =>
                this.HasNextPage ? this.PageNumber + 1 : this.TotalPages;
        public int PreviousPageNumber =>
                this.HasPreviousPage ? this.PageNumber - 1 : 1;

        public PagingHeader GetHeader()
        {
            return new PagingHeader(
                    this.TotalItems, this.PageNumber,
                    this.PageSize, this.TotalPages);
        }

        public List<TResponse> GetDefaultData<TResponse>() where TResponse : new()
        {
            List<TResponse> result = new List<TResponse>();

            return result;
        }
    }
}
