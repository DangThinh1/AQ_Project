using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostFileStream;
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
    public class PostController : BaseApiController
    {
        #region Fields
        private readonly IPostService _postService;
        #endregion

        #region Ctor

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        #endregion

        #region Methods
        [Route("Post/{id}")]
        [HttpGet]
        public IActionResult GetById(long id)
        {
            var res = _postService.GetPostById(id);
            
            return OkBaseResponse(res);
        }
        [Route("Post")]
        [HttpPost]
        public IActionResult CreateNewPost([FromBody]PostCreateModel model)
        {
            
            var id = _postService.CreateNewPost(model);
            if (id==0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);           
            return OkBaseResponse(id);
        }

        [Route("PostSearch")]
        [HttpGet]
        public IActionResult SearchPost([FromQuery]PostSearchModel model)
        {
            try
            {
                var res = _postService.SearchPost(model);
                if (res != null)
                    return OkBaseResponse(res);
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return ErrorBaseResponse(ex);
            }
        }
        [Route("Post")]
        [HttpPut]
        public IActionResult UpdatePost([FromBody]PostCreateModel model)
        {
            var res = _postService.UpdatePost(model);
            if (res == 0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("Post/{postId}")]
        [HttpDelete]
        public IActionResult DeletePost(int postId)
        {
            var res = _postService.DeletePost(postId);
            if (!res)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("Post/ChangeStatus/{postId}")]
        [HttpPut]
        public IActionResult ChangePostStatus(int postId,bool isActive)
        {
            var res = _postService.ChangeStatusPost(postId,isActive);
            if (!res)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("Post/RestorePost/{postId}")]
        [HttpPut]
        public IActionResult RestorePost(int postId)
        {
            var res = _postService.RestorePost(postId);
            if (!res)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("Post/GetPostDetailNagivation")]
        [HttpGet]
        public IActionResult GetPostDetailNagivation([FromQuery]PostSearchModel model)
        {
            try
            {
                var res = _postService.GetPostNavigationDetail(model);
                if (res != null)
                    return OkBaseResponse(res);

                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return ErrorBaseResponse(ex);
            }
        }
        #endregion
    }
}