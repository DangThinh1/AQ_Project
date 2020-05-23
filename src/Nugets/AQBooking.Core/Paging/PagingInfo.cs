
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AQBooking.Core.Paging
{
    public class PagingInfo
    {
        public int TotalItems { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => this.Page > 1;
        public bool HasNextPage => this.Page < this.TotalPages;
        public int NextPageNumber => this.HasNextPage ? this.Page + 1 : this.TotalPages;
        public int PreviousPageNumber => this.HasPreviousPage ? this.Page - 1 : 1;

        public PagingInfo(int totalItems, int pageNumber, int pageSize, int totalPages)
        {
            this.TotalItems = totalItems;
            this.Page = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

    }
}
