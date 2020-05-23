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
  public class PostCategoryController : BaseApiController
  {
    #region Fields
    private readonly IPostCategoryService _postCategoryService;
    #endregion

    #region Ctor

    public PostCategoryController(IPostCategoryService postCategoryService)
    {
      _postCategoryService = postCategoryService;
    }
    #endregion

    #region Methods
    [Route("PostCategory/{id}")]
    [HttpGet]
    public IActionResult GetById(int id)
    {
      var res = _postCategoryService.GetPostCategoriesById(id);
      return OkBaseResponse(res);
    }
    [Route("PostCategory")]
    [HttpPost]
    public IActionResult CreateNewPostCategory([FromBody]PostCategoriesCreateModel model)
    {
      var res = _postCategoryService.CreatePostCategories(model);
      if (res == 0)
        return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
      return OkBaseResponse(res);
    }

    [Route("PostCategorySearch")]
    [HttpPost]
    public IActionResult SearchPostCategory(PostCategoriesSearchModel searchModel)
    {
      var res = _postCategoryService.GetPostCategories(searchModel);
      if (res != null)
        return OkBaseResponse(res);
      return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
    }
    [Route("PostCategory")]
    [HttpPut]
    public IActionResult PostCategory([FromBody]PostCategoriesCreateModel model)
    {
      var res = _postCategoryService.UpdatePostCategories(model);
      if (res == 0)
        return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
      return OkBaseResponse(res);
    }

    [Route("PostCategory/{categoryId}")]
    [HttpDelete]
    public IActionResult DeletePost(int categoryId)
    {
      var res = _postCategoryService.DeletePostCategories(categoryId);
      return OkBaseResponse(res);
    }

    [Route("PostCategoryParentLst")]
    [HttpGet]
    public IActionResult GetParentLst()
    {
      var res = _postCategoryService.GetParentLst();
      if (res != null)
        return OkBaseResponse(res);
      return ErrorBaseResponse(System.Net.HttpStatusCode.NoContent);
    }
    #endregion
  }
}