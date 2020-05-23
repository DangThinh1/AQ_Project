using AQBooking.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Paging;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPostService
    {
        ApiActionResult CreateNewPost(PostCreateModel model);
        ApiActionResult UpdatePost(PostCreateModel model);
        IPagedList<PostViewModel> SearchPost(PostSearchModel searchModel);
        bool DeletePost(int postID);
    }
}
