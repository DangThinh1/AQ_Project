using AQBooking.Admin.Core.Models.PostDetail;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
   public interface IPostDetailService
    {

        PostDetailViewModel GetPostDetailById(long postDetailId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="langugeId"></param>
        /// <returns></returns>
        PostDetailViewModel GetPostDetailViewByPostIdAndLanguageId(int postId, int langugeId);

       /// <summary>
       /// Get a post detail 
       /// </summary>
       /// <param name="postId">post id</param>
       /// <param name="langugeId">language id</param>
       /// <returns></returns>
        PostDetailCreateModel GetPostDetailByPostIdAndLanguageId(long postId, int langugeId);
        /// <summary>
        /// Get list Post detail 
        /// </summary>
        /// <param name="postId">post id</param>
        /// <returns>List of post</returns>
        List<PostDetailLanguageViewModel> GetLanguageIdsByPostId(int postId);
        /// <summary>
        /// create new post detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long CreatePostDetail(PostDetailCreateModel model);
        /// <summary>
        /// Update post detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        long UpdatePostDetail(PostDetailCreateModel model);
        /// <summary>
        /// delete a post detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        bool DeletePostDetail(int id);

      
    }
}
