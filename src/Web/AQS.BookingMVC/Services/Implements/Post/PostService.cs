using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Paging;
using AQS.BookingMVC.Infrastructure.AQPagination;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Post;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Post
{
    public class PostService : ServiceBase, IPostService
    {
        #region Fields
        private readonly AdminApiUrl _adminApiUrl;
        private string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiQVEgQm9va2luZyIsIkVtYWlsIjoiY21zQGFxYm9va2luZy5jb20iLCJVSUQiOiI5NzZmNjc0Ny1kOTIyLTQ4MmQtOWQzMi03MDQ2ZDZmOWIwMTkiLCJSb2xlSWQiOiIyIiwiUm9sZSI6IlN1cGVyIGFkbWluaXN0cmF0b3IiLCJVbmlxdWVJZCI6Ikk5NDg1STdZRTc3QyIsIkFjY2Vzc1Rva2VuIjoiIiwiUmVmcmVzaFRva2VuIjoiVFhWWjIyN01OWkdKIiwiQWNjb3VudFR5cGUiOiIxIiwiVG9rZW5FZmZlY3RpdmVEYXRlIjoiNS83LzIwMjAgMTo0MDo1OCBQTSIsIlRva2VuRWZmZWN0aXZlVGltZVN0aWNrIjoiNjM3MjQ0NTU2NTg2OTA2MjkwIiwibmJmIjoxNTg4ODMzNjU4LCJleHAiOjE5NDg4MzM2NTgsImlhdCI6MTU4ODgzMzY1OCwiaXNzIjoiQVFJZGVudGl0eVNlcnZlciIsImF1ZCI6IkFRSWRlbnRpdHlTZXJ2ZXIifQ.Vl0qpNa-YoAc5nv_g0eKfUncP6fKupFlo52aYoJS5G4";
        #endregion

        #region Ctor
        public PostService(IOptions<AdminApiUrl> adminApiUrlOption) : base()
        {
            _adminApiUrl = adminApiUrlOption.Value;
        }
        #endregion

        #region Method
        /// <summary>
        /// Search all available travel blog
        /// </summary>
        /// <param name="searchModel">Search config</param>
        /// <returns>Travel blog</returns>
        public async Task<BaseResponse<PagedListClient<PostViewModel>>> Search(PostSearchModel searchModel)
        {
            try
            {
                var apiExecute = new APIExcute(AuthenticationType.Bearer);

                var url = string.Format("{0}{1}",
                    (_baseAdminApiUrl + _adminApiUrl.Post.Search),
                    ConvertToUrlParameter(searchModel));

                var response = await apiExecute.GetData<PagedListClient<PostViewModel>>(url, token: _token);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedListClient<PostViewModel>>.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get Post Detail
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="languageId"></param>
        /// <returns>Post Detail</returns>
        public async Task<BaseResponse<PostDetailViewModel>> GetPostDetail(long postId, int languageId)
        {
            try
            {
                var apiExecute = new APIExcute(AuthenticationType.Bearer);
                var url = string.Format("{0}{1}/{2}/{3}",
                    _baseAdminApiUrl,
                    _adminApiUrl.Post.PostDetail,
                    postId,
                    languageId);

                var response = await apiExecute.GetData<PostDetailViewModel>(url, token: _token);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PostDetailViewModel>.InternalServerError(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PostDetailViewModel>> GetPostDetail(long postDetailId)
        {
            try
            {
                var apiExecute = new APIExcute(AuthenticationType.Bearer);
                var url = string.Format("{0}{1}/{2}",
                    _baseAdminApiUrl,
                    _adminApiUrl.Post.PostDetailById,
                    postDetailId
                   );

                var response = await apiExecute.GetData<PostDetailViewModel>(url, token: _token);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<PostDetailViewModel>.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get Post Nagivation
        /// </summary>
        /// <param name="postSearchModel">Post search condition</param>
        /// <returns>Post Nagivation</returns>
        public async Task<BaseResponse<PostNavigationDetailViewModel>> GetPostNagivation(PostSearchModel postSearchModel)
        {
            try
            {
                var apiExecute = new APIExcute(AuthenticationType.Bearer);
                var url = string.Format("{0}{1}",
                    (_baseAdminApiUrl + _adminApiUrl.Post.PostNagivation),
                    ConvertToUrlParameter(postSearchModel));
                var response = await apiExecute.GetData<PostNavigationDetailViewModel>(url, token: _token);

                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<PostNavigationDetailViewModel>.InternalServerError(ex);
            }
        }
        #endregion
    }
}
