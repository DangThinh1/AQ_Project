using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AQBooking.Admin.Core.Paging
{
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public PagedList()
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
        {
            var total = source.Count();
            this.TotalItems = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            if (getOnlyTotalCount)
                return;
            this.Data = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalItems = source.Count;
            TotalPages = TotalItems / pageSize;

            if (TotalItems % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Data = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalItems = totalCount;
            TotalPages = TotalItems / pageSize;

            if (TotalItems % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Data = source;
        }

        /// <summary>
        /// Page index
        /// </summary>
        [JsonProperty("pageIndex")]
        public int PageIndex { get; }

        /// <summary>
        /// Page size
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; }

        /// <summary>
        /// Total count
        /// </summary>
        [JsonProperty("totalItems")]
        public int TotalItems { get; }

        /// <summary>
        /// Total pages
        /// </summary>
        [JsonProperty("totalPages")]
        public int TotalPages { get; }

        /// <summary>
        /// Has previous page
        /// </summary>
        [JsonProperty("hasPreviousPage")]
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// Has next page
        /// </summary>
        [JsonProperty("hasNextPage")]
        public bool HasNextPage => PageIndex + 1 < TotalPages;

        /// <summary>
        /// Return data
        /// </summary>
        /// <returns></returns>
        [JsonProperty("data")]
        public object Data { get; }
    }
}