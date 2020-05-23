using System;
using System.Collections.Generic;
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
    public class PostFileStreamController : BaseApiController
    {
        #region Fields
        private readonly IPostFileStreamService _postFileStreamService;
        #endregion

        #region Ctor

        public PostFileStreamController(IPostFileStreamService postFileStreamService)
        {
            _postFileStreamService = postFileStreamService;
        }
        #endregion

        #region Methods
        [Route("PostFileStream/{id}")]
        [HttpGet]
        public IActionResult GetById(long id)
        {
            var res = _postFileStreamService.GetPostFileStreamById(id);
            
            return OkBaseResponse(res);
        }    

        [Route("PostFileStreamList/{postId}")]
        [HttpGet]
        public IActionResult GetListFileStreams(int postId)
        {
            var res = _postFileStreamService.GetPostFileStreamByPostId(postId);
            return OkBaseResponse(res);
        }
        [Route("PostFileStream")]
        [HttpPost]
        public IActionResult CreateNewPostFileStream([FromBody]PostFileStreamCreateModel model)
        {
            var res = _postFileStreamService.CreatePostFileStream(model);
            if (res == 0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
            return OkBaseResponse(res);
        }
        [Route("PostFileStream")]
        [HttpPut]
        public IActionResult UpdatePostFileStream([FromBody]PostFileStreamCreateModel model)
        {
            var res = _postFileStreamService.UpdatePostFileStream(model);
            if (res == 0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
            return OkBaseResponse(res);
        }

        [Route("PostFileStream/{id}")]
        [HttpDelete]
        public IActionResult DeletePost(int id)
        {
            var res = _postFileStreamService.DeletePostFileStream(id);
            return OkBaseResponse(res);
        }
        
        #endregion
    }
}