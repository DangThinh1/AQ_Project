using AQBooking.Core.Helpers;
using AQDiningPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AQDiningPortal.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class RestaurantAttributesController : ControllerBase
    {
        private readonly IRestaurantAttributeService _attributeService;
        public RestaurantAttributesController(IRestaurantAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        /// <summary>
        /// Return list restaurant attribute by category id
        /// </summary>
        /// <param name="categoryFid">CategoryFid in int format</param>
        /// <returns>List of RestaurantAttributeViewModel</returns>
        [HttpGet("RestaurantAttributes/Category/{categoryFid}")]
        public IActionResult GetAllByCategoryFid(int categoryFid)
        {
            var result = _attributeService.GetAllByCategoryFid(categoryFid);
            if (result.ResponseData == null || result.ResponseData.Count == 0)
                return this.NotFoundResponse();
            return Ok(result);
        }

        /// <summary>
        /// Return list restaurant attribute by category id
        /// </summary>
        /// <param name="listCategoryIds">List CategoryFid in int format</param>
        /// <returns>List of RestaurantAttributeViewModel</returns>
        [HttpGet("RestaurantAttributes/Categories")]
        public IActionResult GetAllByCategoryFids([FromQuery]List<int> listCategoryIds)
        {
            var result = _attributeService.GetAllByListCategoryFids(listCategoryIds);
            if (result.ResponseData == null || result.ResponseData.Count == 0)
                return this.NotFoundResponse();
            return Ok(result);
        }

        /// <summary>
        /// Return list restaurant attribute by category id=[1,2,4]
        /// </summary>
        /// <returns>List of RestaurantAttributeViewModel</returns>
        [HttpGet("RestaurantAttributes/Categories/ForSearch")]
        public IActionResult GetAllForSearch()
        {
            var listCategoryIds = new List<int> { 1, 2, 4 };
            var result = _attributeService.GetAllByListCategoryFids(listCategoryIds);
            if (result.ResponseData == null || result.ResponseData.Count == 0)
                return this.NotFoundResponse();
            return Ok(result);
        }

    }
}