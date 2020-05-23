using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtMerchant;
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
    public class YachtMerchantController : ControllerBase
    {
        #region Fields
        private readonly IYachtMerchantService _yachtMerchantService;
        #endregion

        #region Ctor
        public YachtMerchantController(IYachtMerchantService YachtMerchantService)
        {
            _yachtMerchantService = YachtMerchantService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// API Search Yacht merchant 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchants")]
        [ProducesResponseType(typeof(YachtMerchantViewModel), 200)]
        public IActionResult SearchYachtMerchants([FromQuery]YachtMerchantSearchModel searchModel = null)
        {
            try
            {
                var result = _yachtMerchantService.SearchYachtMerchant(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get Yacht merchant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchants/{id}")]
        public IActionResult FindYachtMerchant(int id)
        {
            try
            {
                var result = _yachtMerchantService.FindYachtMerchantByIdAsync(id);
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
        /// API Create Yacht merchant
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtMerchants")]
        public IActionResult CreateYachtMerchant([FromBody]YachtMerchantCreateModel createModel)
        {
            try
            {
                var result = _yachtMerchantService.CreateYachtMerchant(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Update Yacht merchant 
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtMerchants")]
        public IActionResult UpdateYachtMerchant([FromBody]YachtMerchantCreateModel updateModel)
        {
            try
            {
                var result = _yachtMerchantService.UpdateYachtMerchantAsync(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API Delete Yacht merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtMerchants/{id}")]
        public IActionResult DeleteYachtMerchant(int id)
        {
            try
            {
                var result = _yachtMerchantService.DeleteYachtMerchant(id);
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