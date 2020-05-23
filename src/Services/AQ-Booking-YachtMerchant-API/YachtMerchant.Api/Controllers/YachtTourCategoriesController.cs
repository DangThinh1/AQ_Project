using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Conts;
using System;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtTourCategoriesController : ControllerBase
    {
        private readonly IYachtTourCategoryService _yachtTourCategoryService;
        public YachtTourCategoriesController(IYachtTourCategoryService yachtTourCategoryService)
        {
            _yachtTourCategoryService = yachtTourCategoryService;
        }

        [HttpGet("YachtTourCategories")]
        public IActionResult All()
        {
            
            var result = _yachtTourCategoryService.GetAll();
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpGet("YachtTourCategories/{categoryId}/Language/{langId}")]
        public IActionResult GetCategoryInfo(string categoryId, int langId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
    }
}