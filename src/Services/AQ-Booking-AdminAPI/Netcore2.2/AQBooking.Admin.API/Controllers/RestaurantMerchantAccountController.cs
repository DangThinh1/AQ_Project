using AQBooking.Admin.Core.Models.RestaurantMerchantAccount;
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
    public class RestaurantMerchantAccountController : ControllerBase
    {
        #region Fields

        private readonly IRestaurantMerchantAccService _restaurantMerchantAccService;

        #endregion Fields

        #region Ctor

        public RestaurantMerchantAccountController(IRestaurantMerchantAccService restaurantMerchantAccService)
        {
            this._restaurantMerchantAccService = restaurantMerchantAccService;
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// API Search restaurant merchant user
        /// </summary>
        /// <param name="searchModel"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccounts")]
        [ProducesResponseType(typeof(RestaurantMerchantAccViewModel), 200)]
        public IActionResult SearchRestaurantMerchantAcc([FromQuery]RestaurantMerchantAccSearchModel searchModel)
        {
            try
            {
                var result = _restaurantMerchantAccService.SearchRestaurantMerchantAcc(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get restaurant merchant user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccounts/{id}")]
        public IActionResult FindRestaurantMerchantAcc(int id)
        {
            try
            {
                var result = _restaurantMerchantAccService.FindRestaurantMerchantAccById(id);
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
        /// API Create new restaurant merchant user
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RestaurantMerchantAccounts")]
        public IActionResult CreateRestaurantMerchantAcc([FromBody]RestaurantMerchantAccCreateModel createModel)
        {
            try
            {
                var result = _restaurantMerchantAccService.CreateRestaurantMerchantAcc(createModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API update restaurant merchant user
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RestaurantMerchantAccounts")]
        public IActionResult UpdateRestaurantMerchantAcc([FromBody]RestaurantMerchantAccCreateModel updateModel)
        {
            try
            {
                var result = _restaurantMerchantAccService.UpdateRestaurantMerchantAcc(updateModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API delete restaurant merchant user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RestaurantMerchantAccounts/{id}")]
        public IActionResult DeleteRestaurantMerchantAcc(int id)
        {
            try
            {
                var result = _restaurantMerchantAccService.DeleteRestaurantMerchantAcc(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get restaurant account management by merchant id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RestaurantMerchantAccounts/ByMerchantId/{merchantId}")]
        public IActionResult GetRestaurantMerchantAccByMerchantId(int merchantId)
        {
            try
            {
                var result = _restaurantMerchantAccService.GetRestaurantMerchantAccByMerchatnId(merchantId);
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