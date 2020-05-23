using System;
namespace AQBooking.Admin.Core.Models.PostDetail
{
    public class PostDetailCreateModel
    {
        public long Id { get; set; }
        public string UniqueId { get; set; }
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
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
