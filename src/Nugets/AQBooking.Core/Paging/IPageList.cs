namespace AQBooking.Core.Paging
{
    public interface IPageList
    {
        int TotalItems { get; set; }
        int TotalPages { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
    }
}