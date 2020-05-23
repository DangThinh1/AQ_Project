using AQBooking.Admin.Core.Models.RestaurantMerchantAccountMgt;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class RestaurantMerchantAccountMgtController : ControllerBase
    {
        #region Fields

        private readonly IRestaurantMerchantAccMgtService _restaurantMerchantAccMgtService;

        #endregion Fields

        #region Ctor

        public RestaurantMerchantAccountMgtController(IRestaurantMerchantAccMgtService restaurantMerchantAccMgtService)
        {
            this._restaurantMerchantAccMgtService = restaurantMerchantAccMgtService;
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// API Search restaurant merchant account management
        /// </summary>
        /// <param name="searchModel"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccountMgts")]
        [ProducesResponseType(typeof(RestaurantMerchantAccMgtViewModel), 200)]
        public IActionResult SearchRestaurantMerchantAccMgt([FromQuery]RestaurantMerchantAccMgtSearchModel searchModel)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.SearchRestaurantMerchantAccMgt(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get restaurant merchant account management by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccountMgts/{id}")]
        public IActionResult FindRestaurantMerchantAccMgt(int id)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.FindRestaurantMerchantAccMgtById(id);
                if (result == null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Create new restaurant merchant account management
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RestaurantMerchantAccountMgts")]
        public IActionResult CreateRestaurantMerchantAccMgt([FromBody]RestaurantMerchantAccMgtCreateModel createModel)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.CreateRestaurantMerchantAccMgt(createModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API update restaurant merchant account management
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RestaurantMerchantAccountMgts")]
        public IActionResult UpdateRestaurantMerchantAccMgt([FromBody]RestaurantMerchantAccMgtCreateModel updateModel)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.UpdateRestaurantMerchantAccMgt(updateModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API delete restaurant merchant account mamagement by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RestaurantMerchantAccountMgts/{id}")]
        public IActionResult DeleteRestaurantMerchantAccMgt(int id)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.DeleteRestaurantMerchantAccMgt(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get restaurant merchant account management by merchant id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccountMgts/ByMerchantId/{merchantId}")]
        public IActionResult GetRestaurantMerchantAccMgtByMerchantId(int merchantId)
        {
            try
            {
                var result = _restaurantMerchantAccMgtService.GetRestaurantMerchantAccMgtByMerchantId(merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        #endregion Methods
    }
}