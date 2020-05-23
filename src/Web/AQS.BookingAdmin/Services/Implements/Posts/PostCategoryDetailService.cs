using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PostCategories;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using ExtendedUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Posts
{
    public class PostCategoryDetailService : BaseService, IPostCategoryDetailService
    {
        #region Field    
        #endregion

        #region Ctor
        public PostCategoryDetailService()
        {
        }
        #endregion

        #region Method
        public async Task<BaseResponse<object>> CreatePostCategory(PostCategoryDetailCreateModel model)
        {
            try
            {
                string apiUrl = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.PostCategoryDetail}";
                var req = new BaseRequest<PostCategoryDetailCreateModel>(model);
                var res = await _aPIExcute.PostData<object, PostCategoryDetailCreateModel>(url: apiUrl, requestParams: req, token: Token);
                if (!res.IsSuccessStatusCode)
                    return null;
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<bool> DeletePostCategory(int id)
        {
            try
            {
                var res = await _aPIExcute.PostData<bool, object>(
                                url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.PostCategoryDetail}/{id}",
                                method: HttpMethodEnum.DELETE,
                                requestParams: null,
                                token: Token);
                if (!res.IsSuccessStatusCode)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PostCategoryDetailViewModel> GetListPostCategories(int categoryId, int languageId)
        {
            var res = await _aPIExcute.GetData<PostCategoryDetailViewModel>(
                           $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.PostCategoryDetail}/{categoryId}/{languageId}", null, Token);
            if (!res.IsSuccessStatusCode)
                return null;
            return res.ResponseData;
        }

        public async Task<BaseResponse<List<PostCategoryDetailViewModel>>> GetPostCateDetailByPostCateId(int postId)
        {
            var res = await _aPIExcute.GetData<List<PostCategoryDetailViewModel>>(
                           $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.GetPostCateDetailByPostCateId}{postId}", null, Token);
            return res;
        }

        public async Task<BaseResponse<object>> UpdatePostCategory(PostCategoryDetailCreateModel model)
        {
            try
            {
                string apiUrl = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.PostCategoryDetail}";
                var req = new BaseRequest<PostCategoryDetailCreateModel>(model);
                var res = await _aPIExcute.PostData<object, PostCategoryDetailCreateModel>(url: apiUrl, HttpMethodEnum.PUT, requestParams: req, token: Token);
                if (!res.IsSuccessStatusCode)
                    return null;
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        public async Task<bool> CheckPostCateDuplicate(PostCategoriesCreateModel model)
        {
            try
            {
                string apiUrl = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoryDetailAPI.CheckPostCateDuplicate}";
                PostCategoryDetailCreateModel modelDetail = new PostCategoryDetailCreateModel();
                modelDetail.PostCategoryFid = model.Id;
                modelDetail.LanguageFid = model.LanguageFid;
                modelDetail.Id = model.PostCateDetailId;
                var req = new BaseRequest<PostCategoryDetailCreateModel>(modelDetail);
                var res = await _aPIExcute.PostData<object, PostCategoryDetailCreateModel>(url: apiUrl, requestParams: req, token: Token);
                if (!res.IsSuccessStatusCode)
                    return res.ResponseData.ToBool();
                return res.ResponseData.ToBool();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
