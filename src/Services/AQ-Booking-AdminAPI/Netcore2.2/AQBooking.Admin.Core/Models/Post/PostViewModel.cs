using System;
using System.Collections.Generic;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
namespace AQBooking.Admin.Core.Models.Post
{
    public class PostViewModel
    {
        #region Fields
        public long Id { get; set; }
        public int PostCategoryFid { get; set; }
        public string PostCategoryResKey { get; set; }
        public string DefaultTitle { get; set; }
        public short TimeToRead { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public PostDetailViewModel postDetail;
        public List<PostFileStreamViewModel> postFileStream;
        #endregion
        #region Ctor
        public PostViewModel()
        {
            postDetail = new PostDetailViewModel();
            postFileStream = new List<PostFileStreamViewModel>();
        }
        #endregion
    }
}
