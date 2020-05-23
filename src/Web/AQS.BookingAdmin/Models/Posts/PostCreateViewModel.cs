using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Posts
{
    public class PostCreateViewModel
    {
        public PostCreateViewModel()
        {
            PostInfo = new PostCreateModel();
            PostDetail = new PostDetailCreateModel();
            ListImgDescriptions = new List<string>();
        }
        public string ThumbImgName { get; set; }
        public List<string> ListImgDescriptions { get; set; }
        public string FriendlyUrl { get; set; }
        public int LanguageId { get; set; }
        public PostCreateModel PostInfo { get; set; }
        public PostDetailCreateModel PostDetail { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Languages { get; set; }
    }
}
