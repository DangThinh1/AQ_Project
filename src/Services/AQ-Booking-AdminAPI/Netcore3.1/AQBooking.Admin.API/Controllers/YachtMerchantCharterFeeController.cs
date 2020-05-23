using System;
using AQBooking.Admin.Core.Models.YachtMerchantCharterFee;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class YachtMerchantCharterFeeController : ControllerBase
    {
        #region Fields
        private readonly IYachtMerchantCharterFeeService _yachtMerchantCharterFeeService;
        #endregion

        #region Ctor
        public YachtMerchantCharterFeeController(
            IYachtMerchantCharterFeeService yachtMerchantCharterFeeService
            )
        {
            _yachtMerchantCharterFeeService = yachtMerchantCharterFeeService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all yacht merchant charter fee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantCharterFee")]
        public IActionResult GetAllYachtMerchantCharterFee()
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.GetAllYachtMerchantCharterFee();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get list yacht merchant charter fee by merchantid
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantCharterFee/ByMerchantId/{merchantId}")]
        public IActionResult GetYachtMerchantCharterFeeByMerchantId(int merchantId)
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.GetYachtMerchantCharterFeeByMerchantId(merchantId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        
        /// <summary>
        /// Get yacht merchant charter fee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantCharterFee/{id}")]
        public IActionResult GetYachtMerchantCharterFeeById(int id)
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.GetYachtMerchantCharterFeeById(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        
        /// <summary>
        /// Create new yacht merchant charter fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtMerchantCharterFee")]
        public IActionResult CreateYachtMerchantCharterFee(YahctMerchantCharterFeeCreateModel model)
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.CreateYachtMerchantCharterFee(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Update yacht merchant charter fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtMerchantCharterFee")]
        public IActionResult UpdateYachtMerchantCharterFee(YachtMerchantCharterFeeUpdateModel model)
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.UpdateYachtMerchantCharterFee(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Delete yacht merchant charter fee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtMerchantCharterFee/{id}")]
        public IActionResult DeleteYachtMerchantCharterFee(int id)
        {
            try
            {
                var result = _yachtMerchantCharterFeeService.DeleteYachtMerchantCharterFee(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        #endregion
    }
}