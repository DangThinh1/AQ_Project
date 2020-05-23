using AQBooking.Admin.Core.Models.Post;
using AQS.BookingMVC.Infrastructure.AQPagination;

namespace AQS.BookingMVC.Areas.TravelBlog.Models
{
    public class TravelBlogViewModel
    {
        public PagedListClient<PostViewModel> TravelBlogItems { get; set; }
    }
}
