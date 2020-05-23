using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Paging;
using AQS.BookingMVC.Infrastructure.AQPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Post
{
    public interface IPostService
    {
        public Task<BaseResponse<PagedListClient<PostViewModel>>> Search(PostSearchModel postSearchModel);
        public Task<BaseResponse<PostDetailViewModel>> GetPostDetail(long postId, int languageId);
        public Task<BaseResponse<PostDetailViewModel>> GetPostDetail(long postDetailId);
        public Task<BaseResponse<PostNavigationDetailViewModel>> GetPostNagivation(PostSearchModel postSearchModel);
    }
}
