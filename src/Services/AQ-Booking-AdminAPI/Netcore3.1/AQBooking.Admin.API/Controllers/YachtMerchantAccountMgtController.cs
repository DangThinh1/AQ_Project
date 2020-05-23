using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtMerchantAccountMgt;
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
    public class YachtMerchantAccountMgtController : ControllerBase
    {
        #region Fields
        private readonly IYachtMerchantAccMgtService _yachtMerchantAccMgtService;
        #endregion

        #region Ctor
        public YachtMerchantAccountMgtController(IYachtMerchantAccMgtService YachtMerchantAccMgtService)
        {
            this._yachtMerchantAccMgtService = YachtMerchantAccMgtService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// API Search Yacht merchant account management
        /// </summary>
        /// <param name="searchModel"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccountMgts")]
        [ProducesResponseType(typeof(YachtMerchantAccMgtViewModel), 200)]
        public IActionResult SearchYachtMerchantAccMgt([FromQuery]YachtMerchantAccMgtSearchModel searchModel)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.SearchYachtMerchantAccMgt(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get Yacht merchant account management by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccountMgts/{id}")]
        public IActionResult FindYachtMerchantAccMgt(int id)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.FindYachtMerchantAccMgtById(id);
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
        /// API Create new Yacht merchant account management
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtMerchantAccountMgts")]
        public IActionResult CreateYachtMerchantAccMgt([FromBody]YachtMerchantAccMgtCreateModel createModel)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.CreateYachtMerchantAccMgt(createModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API update Yacht merchant account management
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtMerchantAccountMgts")]
        public IActionResult UpdateYachtMerchantAccMgt([FromBody]YachtMerchantAccMgtCreateModel updateModel)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.UpdateYachtMerchantAccMgt(updateModel).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API delete Yacht merchant account mamagement by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtMerchantAccountMgts/{id}")]
        public IActionResult DeleteYachtMerchantAccMgt(int id)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.DeleteYachtMerchantAccMgt(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant account management by merchant id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccountMgts/ByMerchantId/{merchantId}")]
        public IActionResult GetYachtMerchantAccMgtByMerchantId(int merchantId)
        {
            try
            {
                var result = _yachtMerchantAccMgtService.GetYachtMerchantAccMgtByMerchantId(merchantId);
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