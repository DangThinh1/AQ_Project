using APIHelpers.Response;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
using AQBooking.Admin.Core.Paging;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Posts
{
    public interface IPostService
    {
        #region Post
        Task<BaseResponse<PostCreateModel>> GetPostById(int id);
        /// <summary>
        /// Search post list
        /// </summary>
        /// <param name="searchModel">search paramter model</param>
        /// <returns>Paged list of post</returns>
        Task<BaseResponse<PagedListClient<PostViewModel>>> SearchPost(PostSearchModel searchModel);
        /// <summary>
        /// Create New Post
        /// </summary>
        /// <param name="model">Post infomation</param>
        /// <returns>BaseResponse with api action result</returns>
        Task<BaseResponse<long>> CreateNewPost(PostCreateModel model);
        /// <summary>
        /// Update post
        /// </summary>
        /// <param name="model">Post infomation include Id</param>
        /// <returns>BaseResponse with api action result</returns>
        Task<BaseResponse<long>> UpdatePost(PostCreateModel model);
        /// <summary>
        /// Delete post
        /// </summary>
        /// <param name="postID">post id</param>
        /// <returns>true if success else false</returns>
        Task<BaseResponse<bool>> DeletePost(int postID);
        /// <summary>
        /// Restore Post
        /// </summary>
        /// <param name="postID">Post id</param>
        /// <returns>true if success else false</returns>
        Task<BaseResponse<bool>> RestorePost(int postID);
        /// <summary>
        /// change post status
        /// </summary>
        /// <param name="postID">postid</param>
        /// <param name="isActive">true active, false deactive</param>
        /// <returns></returns>
        Task<BaseResponse<bool>> ChangePostStatus(int postID, bool isActive);
        #endregion

        #region Post Detail
        Task<BaseResponse<List<PostDetailLanguageViewModel>>> GetLanguageIdsByPostId(long postId);
        Task<BaseResponse<PostDetailCreateModel>> GetPostDetailByPostIdAndLanguageId(long postId, int languageId);
        Task<BaseResponse<long>> CreateNewPostDetail(PostDetailCreateModel model);
        Task<BaseResponse<long>> UpdatePostDetail(PostDetailCreateModel model);
        Task<BaseResponse<List<PostFileStreamViewModel>>> GetFileStreamOfPostDetail(long postDetailId);
        #endregion
    }
}
