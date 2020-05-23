namespace AQBooking.Admin.Core.Models.PostDetail
{
    public class PostNavigationDetailViewModel
    {
        public NavigationInfo NextPost { get; set; }
        public NavigationInfo PreviousPost { get; set; }
    }
    public class NavigationInfo
    {
        public long PostID { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; }
    }
}
