namespace AQBooking.Admin.Core.Models.PostDetail
{
    public class PostDetailViewModel
    {
        public long PostFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Body { get; set; }
        public string KeyWord { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
    }
}
