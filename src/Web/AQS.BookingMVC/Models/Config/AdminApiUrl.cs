namespace AQS.BookingMVC.Models.Config
{
    public class AdminApiUrl
    {
        public Post Post {get;set;}
        
    }
    public class Post
    {
        public string Search { get; set; }
        public string Subscribe { get; set; }
        public string PostDetail { get; set; }
        public string PostDetailById { get; set; }
        public string PostNagivation { get; set; }
    }
    
}
