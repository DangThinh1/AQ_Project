using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostDetailController : BaseApiController
    {
        #region Fields
        private readonly IPostDetailService _postDetailService;
        #endregion

        #region Ctor

        public PostDetailController(IPostDetailService postDetailService)
        {
            _postDetailService = postDetailService;
        }
        #endregion

        #region Methods

        [Route("PostDetail/{postDetailId}")]
        [HttpGet]
        public IActionResult GetPostDetailById(int postDetailId)
        {
            var res = _postDetailService.GetPostDetailById(postDetailId);
            return OkBaseResponse(res);
        }

        [Route("PostDetail/LanguageIds/{postId}")]
        [HttpGet]
        public IActionResult GetLanguageIdsByPostId(int postId)
        {
            var res = _postDetailService.GetLanguageIdsByPostId(postId);
            return OkBaseResponse(res);
        }
        [Route("PostDetail/{postId}/{languageId}")]
        [HttpGet]
        public IActionResult GetByPostIdAndLanguage(int postId,int languageId)
        {
            var res = _postDetailService.GetPostDetailByPostIdAndLanguageId(postId, languageId);
            return OkBaseResponse(res);
        }
        [AllowAnonymous]
        [Route("PostDetailView/{postId}/{languageId}")]
        [HttpGet]
        public IActionResult GetViewByPostIdAndLanguage(int postId, int languageId)
        {
            var res = _postDetailService.GetPostDetailViewByPostIdAndLanguageId(postId, languageId);
            return OkBaseResponse(res);
        }
        [Route("PostDetail")]
        [HttpPost]
        public IActionResult CreateNewPost([FromBody]PostDetailCreateModel model)
        {
            var res = _postDetailService.CreatePostDetail(model);
            if (res==0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
            return OkBaseResponse(res);
        }       
        [Route("PostDetail")]
        [HttpPut]
        public IActionResult UpdatePostDetail([FromBody]PostDetailCreateModel model)
        {
            var res = _postDetailService.UpdatePostDetail(model);
            if (res == 0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("PostDetail/{postId}")]
        [HttpDelete]
        public IActionResult DeletePost(int postId)
        {
            var res = _postDetailService.DeletePostDetail(postId);
            return OkBaseResponse(res);
        }
        #endregion
    }
}