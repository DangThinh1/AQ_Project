using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PostCategories;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Constants;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Models.Posts;
using AQS.BookingAdmin.Services.Interfaces.Common;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using ExtendedUtility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Posts
{
    public class PostCategoryService : BaseService, IPostCategoryService
    {

        #region Fields
        private readonly ICommonValueService _commonValueService;
        #endregion

        #region Ctor
        public PostCategoryService(ICommonValueService commonValueService) : base()
        {
            _commonValueService = commonValueService;
        }
        #endregion

        #region Methods
        public async Task<List<PostCategoriesViewModel>> GetParentLst()
        {
            var res = await _aPIExcute.GetData<List<PostCategoriesViewModel>>($"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.PostCategoryParentLst}", null, Token);
            if (!res.IsSuccessStatusCode)
                return null;
            return res.ResponseData;
        }

        public async Task<List<PostCategoryModel>> GetListPostCategories()
        {
            var commonValues = await _commonValueService.GetCommonValuesByGroupName(PostCategoryConstant.POST_CATEGORY_COMMON_VALUE_GROUP);
            return commonValues.Select(commonValues => new PostCategoryModel
            {
                Id = commonValues.Id,
                Name = commonValues.Text,
                ResourceKey = commonValues.ResourceKey
            }).ToList();
        }

        public async Task<BaseResponse<object>> CreatePostCategory(PostCategoriesCreateModel model)
        {
            try
            {
                string apiUrl = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.PostCategories}";
                var req = new BaseRequest<PostCategoriesCreateModel>(model);
                var res = await _aPIExcute.PostData<object, PostCategoriesCreateModel>(url: apiUrl, requestParams: req, token: Token);
                if (!res.IsSuccessStatusCode)
                    return null;
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<object>> UpdatePostCategory(PostCategoriesCreateModel model)
        {
            try
            {
                if (model.LanguageFid == 1)
                    model.DefaultName = model.Name != model.DefaultName ? model.Name : model.DefaultName;

                string apiUrl = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.PostCategories}";
                var req = new BaseRequest<PostCategoriesCreateModel>(model);
                var res = await _aPIExcute.PostData<object, PostCategoriesCreateModel>(url: apiUrl, HttpMethodEnum.PUT, requestParams: req, token: Token);
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
                                url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.PostCategories}/{id}",
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

        public async Task<PostCategoriesViewModel> GetById(int id)
        {
            var res = await _aPIExcute.GetData<PostCategoriesViewModel>($"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.PostCategories}/{id}", null, Token);
            if (!res.IsSuccessStatusCode)
                return null;
            return res.ResponseData;
        }

        public async Task<PagedListClient<PostCategoriesViewModel>> SearchPostCategory(PostCategoriesSearchModel model)
        {
            try
            {
                var req = new BaseRequest<PostCategoriesSearchModel>(model);
                var res = await _aPIExcute.PostData<PagedListClient<PostCategoriesViewModel>, PostCategoriesSearchModel>($"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostCategoriesAPI.Search}", HttpMethodEnum.POST, req, Token);
                if (!res.IsSuccessStatusCode)
                    return null;
                return res.ResponseData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
