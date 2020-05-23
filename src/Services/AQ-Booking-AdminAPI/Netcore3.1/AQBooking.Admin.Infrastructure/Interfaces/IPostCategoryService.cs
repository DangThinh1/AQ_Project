using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PostCategories;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPostCategoryService
    {
        #region Post Categories
        PostCategoriesViewModel GetPostCategoriesById(int id);
        IPagedList<PostCategoriesViewModel> GetPostCategories(PostCategoriesSearchModel searchModel);
        int CreatePostCategories(PostCategoriesCreateModel model);
        int UpdatePostCategories(PostCategoriesCreateModel model);
        bool DeletePostCategories(int id);
        List<PostCategoriesViewModel> GetParentLst();
        #endregion

        #region Post Categories Detail
        PostCategoryDetailViewModel GetPostCategoryDetailByCategoryIdAndLanguageId(int categoryId, int languageId);
        int CreatePostCategoryDetail(PostCategoryDetailCreateModel model);
        int UpdatePostCategoryDetail(PostCategoryDetailCreateModel model);
        bool DeletePostCategoryDetail(int id);
        List<PostCategoryDetailViewModel> GetPostCateDetailByPostCateId(int postId);
        bool CheckPostCategoryDetailDuplicate(PostCategoryDetailCreateModel model);
      #endregion
    }
}
