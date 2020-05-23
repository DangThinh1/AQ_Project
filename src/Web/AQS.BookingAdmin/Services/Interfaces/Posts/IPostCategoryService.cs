using APIHelpers.Response;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PostCategories;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using AQS.BookingAdmin.Models.Posts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Posts
{
  public interface IPostCategoryService
  {
    /// <summary>
    /// Search Post Category
    /// </summary>
    /// <returns></returns>
    Task<List<PostCategoryModel>> GetListPostCategories();
    /// <summary>
    /// Get Post Cate by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PostCategoriesViewModel> GetById(int id);
    /// <summary>
    /// Create Post Category
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<BaseResponse<object>> CreatePostCategory(PostCategoriesCreateModel model);
    /// <summary>
    /// Update Post Category
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<BaseResponse<object>> UpdatePostCategory(PostCategoriesCreateModel model);
    /// <summary>
    /// Delete Post Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeletePostCategory(int id);
    /// <summary>
    /// Search Post Category
    /// </summary>
    /// <returns></returns>
    Task<PagedListClient<PostCategoriesViewModel>> SearchPostCategory(PostCategoriesSearchModel model);

    Task<List<PostCategoriesViewModel>> GetParentLst();
  }
}
