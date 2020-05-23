using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.RestaurantMerchant;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class RestaurantMerchantController : ControllerBase
    {
        #region Fields
        private readonly IRestaurantMerchantService _restaurantMerchantService;
        #endregion

        #region Ctor
        public RestaurantMerchantController(IRestaurantMerchantService restaurantMerchantService)
        {
            _restaurantMerchantService = restaurantMerchantService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// API Search restaurant merchant 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchants")]
        [ProducesResponseType(typeof(RestaurantMerchantViewModel), 200)]
        public IActionResult SearchrantMerchants([FromQuery]RestaurantMerchantSearchModel searchModel = null)
        {
            try
            {
                var result = _restaurantMerchantService.SearchRestaurantMerchant(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get restaurant merchant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchants/{id}")]
        public IActionResult FindRestaurantMerchant(int id)
        {
            try
            {
                var result = _restaurantMerchantService.FindRestaurantMerchantByIdAsync(id);
                if (result == null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Create restaurant merchant
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RestaurantMerchants")]
        public IActionResult CreateRestaurantMerchant([FromBody]RestaurantMerchantCreateModel createModel)
        {
            try
            {
                var result = _restaurantMerchantService.CreateRestaurantMerchant(createModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Update restaurant merchant 
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RestaurantMerchants")]
        public IActionResult UpdateRestaurantMerchant([FromBody]RestaurantMerchantCreateModel updateModel)
        {
            try
            {
                var result = _restaurantMerchantService.UpdateRestaurantMerchantAsync(updateModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Delete restaurant merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RestaurantMerchants/{id}")]
        public IActionResult DeleteRestaurantMerchant(int id)
        {
            try
            {
                var result = _restaurantMerchantService.DeleteRestaurantMerchant(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        #endregion
    }
}