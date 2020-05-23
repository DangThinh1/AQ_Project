using AQBooking.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPostService
    {
        #region Post
        PostCreateModel GetPostById(long id);

        PostNavigationDetailViewModel GetPostNavigationDetail(PostSearchModel searchModel);
         IPagedList<PostViewModel> SearchPost(PostSearchModel searchModel);

        long CreateNewPost(PostCreateModel model);

        long UpdatePost(PostCreateModel model);

        bool DeletePost(int postID);

        bool RestorePost(int postID);
        bool ChangeStatusPost(int postID, bool isActive);
        #endregion
    }
}
