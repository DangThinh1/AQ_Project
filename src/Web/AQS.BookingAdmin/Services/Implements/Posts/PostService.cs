using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
using AQBooking.Admin.Core.Paging;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Posts
{
    public class PostService : BaseService, IPostService
    {

        #region Fields

        #endregion

        #region Ctor
        public PostService()
        {
        }
        #endregion

        #region Methods
        #region Post
        public async Task<BaseResponse<PostCreateModel>> GetPostById(int id)
        {
            try
            {               
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.Post}/{id}";
                var res = await _aPIExcute.GetData<PostCreateModel>(urlRequest, null, Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<PostCreateModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<PagedListClient<PostViewModel>>> SearchPost(PostSearchModel searchModel)
        {
            try
            {
              
                var requestParams = ConvertToUrlParameter(searchModel);
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.Search}{requestParams}";
                var res = await _aPIExcute.GetData<PagedListClient<PostViewModel>>(urlRequest, null, Token);                    
               
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedListClient<PostViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<long>> CreateNewPost(PostCreateModel model)
        {
            try
            {

                var req = new BaseRequest<PostCreateModel>(model);
                var res = await _aPIExcute.PostData<long, PostCreateModel>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.Post}",
                    requestParams: req,
                    token: Token);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<long>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<long>> UpdatePost(PostCreateModel model)
        {
            try
            {

                var req = new BaseRequest<PostCreateModel>(model);
                var res = await _aPIExcute.PostData<long, PostCreateModel>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.Post}",
                    method: HttpMethodEnum.PUT,
                    requestParams: req,
                    token: Token);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<long>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> DeletePost(int postID)
        {
            try
            {
                var res = await _aPIExcute.PostData<bool, object>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.Post}/{postID}",
                   method: HttpMethodEnum.DELETE,
                   requestParams: null,
                    token: Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> RestorePost(int postID)
        {
            try
            {
                var res = await _aPIExcute.PostData<bool, object>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.RestorePost}/{postID}",
                    method: HttpMethodEnum.PUT,
                    requestParams: null,
                    token: Token);

                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> ChangePostStatus(int postID,bool isActive)
        {
            try
            {
                var res = await _aPIExcute.PostData<bool, object>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.ChangeStatus}/{postID}?IsActive={isActive}",
                    method: HttpMethodEnum.PUT,
                    requestParams: null,
                    token: Token);

                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

        #region Post Detail
        public async Task<BaseResponse<long>> CreateNewPostDetail(PostDetailCreateModel model)
        {
            try
            {

                var req = new BaseRequest<PostDetailCreateModel>(model);
                var res = await _aPIExcute.PostData<long, PostDetailCreateModel>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.PostDetail}",
                    requestParams: req,
                    token: Token);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<long>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<long>>UpdatePostDetail(PostDetailCreateModel model)
        {
            try
            {

                var req = new BaseRequest<PostDetailCreateModel>(model);
                var res = await _aPIExcute.PostData<long, PostDetailCreateModel>(
                    url: $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.PostDetail}",
                   method:HttpMethodEnum.PUT,
                    requestParams: req,
                    token: Token);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<long>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<PostDetailCreateModel>> GetPostDetailByPostIdAndLanguageId(long postId, int languageId)
        {
            try
            {
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.PostDetail}/{postId}/{languageId}";
                var res = await _aPIExcute.GetData<PostDetailCreateModel>(urlRequest, null, Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<PostDetailCreateModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<List<PostDetailLanguageViewModel>>> GetLanguageIdsByPostId(long postId)
        {
            try
            {
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.PostDetailLanguageIds}/{postId}";
                var res = await _aPIExcute.GetData<List<PostDetailLanguageViewModel>>(urlRequest, null, Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PostDetailLanguageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<List<PostFileStreamViewModel>>> GetFileStreamOfPostDetail(long postDetailId)
        {
            try
            {
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.FileStreamPostDetail}/{postDetailId}";
                var res = await _aPIExcute.GetData<List<PostFileStreamViewModel>>(urlRequest, null, Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PostFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

        #endregion



    }
}
