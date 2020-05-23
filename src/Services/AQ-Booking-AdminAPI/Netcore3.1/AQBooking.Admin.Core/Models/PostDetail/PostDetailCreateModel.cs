using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AQBooking.Admin.Core.Models.PostDetail
{
    public class PostDetailCreateModel
    {
        public long Id { get; set; }  
        public long PostFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string MetaDescription { get; set; }
        public string Body { get; set; }
        [MaxLength(255)]
        public string KeyWord { get; set; }
        [MaxLength(500)]
        public string ShortDescription { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public List<int> FileDescriptionIds { get; set; } = new List<int>();
        public string FriendlyUrl { get; set; }
    }
}
