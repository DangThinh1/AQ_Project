using APIHelpers.Response;
using AQBooking.Admin.Core.Models.PostCategories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Posts

{
    public interface IPostCategoryDetailService
    {
        /// <summary>
        /// Get Post Category Detail By Id
        /// </summary>
        /// <returns></returns>
        Task<PostCategoryDetailViewModel> GetListPostCategories(int categoryId, int languageId);
        /// <summary>
        /// Create Post Category Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<object>> CreatePostCategory(PostCategoryDetailCreateModel model);
        /// <summary>
        /// Update Post Category Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<object>> UpdatePostCategory(PostCategoryDetailCreateModel model);
        /// <summary>
        /// Delete Post Category Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePostCategory(int id);

        Task<BaseResponse<List<PostCategoryDetailViewModel>>> GetPostCateDetailByPostCateId(int postId);
        Task<bool> CheckPostCateDuplicate(PostCategoriesCreateModel model);
    }
}
