using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
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
        [Route("Post")]
        [HttpPost]
        public IActionResult CreateNewPost([FromBody]PostCreateModel model)
        {
            var res = _postService.CreateNewPost(model);
            if (!res.Succeeded)
                return NotFound();
            return Ok(res);
        }

        [Route("PostSearch")]
        [HttpGet]
        public IActionResult SearchPost([FromQuery]PostSearchModel model)
        {
            try
            {
                var res = _postService.SearchPost(model);
                if (res != null)
                    return Ok(res);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [Route("Post")]
        [HttpPut]
        public IActionResult UpdatePost([FromBody]PostCreateModel model)
        {
            try
            {
                var res = _postService.UpdatePost(model);
                if (!res.Succeeded)
                    return NoContent();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("Post/{postId}")]
        [HttpDelete]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                var res = _postService.DeletePost(postId);
                if (!res)
                    return NoContent();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        #endregion
    }
}