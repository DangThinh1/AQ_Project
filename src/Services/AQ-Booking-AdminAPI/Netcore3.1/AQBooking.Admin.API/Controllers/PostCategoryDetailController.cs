using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostCategories;
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
    public class PostCategoryDetailController : BaseApiController
    {
        #region Fields
        private readonly IPostCategoryService _postCategoryService;
        #endregion

        #region Ctor

        public PostCategoryDetailController(IPostCategoryService postCategoryService)
        {
            _postCategoryService = postCategoryService;
        }
        #endregion

        #region Methods
        [Route("PostCategoryDetail/{categoryId}/{languageId}")]
        [HttpGet]
        public IActionResult GetPostCategoryDetail(int categoryId, int languageId)
        {
            var res = _postCategoryService.GetPostCategoryDetailByCategoryIdAndLanguageId(categoryId, languageId);
            return OkBaseResponse(res);
        }
        [Route("PostCategoryDetail")]
        [HttpPost]
        public IActionResult CreateNewCategoryDetail([FromBody]PostCategoryDetailCreateModel model)
        {
            var res = _postCategoryService.CreatePostCategoryDetail(model);
            if (res == 0)
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
            return OkBaseResponse(res);
        }
        [Route("PostCategoryDetail")]
        [HttpPut]
        public IActionResult UpdatePostCategoryDetail([FromBody]PostCategoryDetailCreateModel model)
        {
            try
            {
                var res = _postCategoryService.UpdatePostCategoryDetail(model);
                if (res == 0)
                    return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
                return OkBaseResponse(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [Route("PostCategoryDetail/{id}")]
        [HttpDelete]
        public IActionResult DeletePostCategoryDetail(int id)
        {
            try
            {
                var res = _postCategoryService.DeletePostCategoryDetail(id);
                return OkBaseResponse(res);
            }
            catch (Exception ex)
            {
                return ErrorBaseResponse(ex.StackTrace.ToString());
            }
        }

        [Route("GetPostCateDetailByPostCateId/{postCateId}")]
        [HttpGet]
        public IActionResult GetPostCateDetailByPostCateId(int postCateId)
        {
            var res = _postCategoryService.GetPostCateDetailByPostCateId(postCateId);
            return OkBaseResponse(res);
        }
        [Route("CheckPostCategoryDetailDuplicate")]
        [HttpPost]
        public IActionResult CheckPostCategoryDetailDuplicate([FromBody]PostCategoryDetailCreateModel model)
        {
            var res = _postCategoryService.CheckPostCategoryDetailDuplicate(model);
            return OkBaseResponse(res);
        }

        #endregion
    }
}