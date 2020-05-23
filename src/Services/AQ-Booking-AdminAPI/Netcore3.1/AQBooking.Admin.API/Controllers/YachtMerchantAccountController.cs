using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtMerchantAccount;
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
    public class YachtMerchantAccountController : ControllerBase
    {
        #region Fields
        private readonly IYachtMerchantAccService _yachtMerchantAccService;
        #endregion

        #region Ctor
        public YachtMerchantAccountController(IYachtMerchantAccService YachtMerchantAccService)
        {
            this._yachtMerchantAccService = YachtMerchantAccService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// API Search Yacht merchant user
        /// </summary>
        /// <param name="searchModel"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccounts")]
        [ProducesResponseType(typeof(YachtMerchantAccViewModel), 200)]
        public IActionResult SearchYachtMerchantAcc([FromQuery]YachtMerchantAccSearchModel searchModel)
        {
            try
            {
                var result = _yachtMerchantAccService.SearchYachtMerchantAcc(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API get Yacht merchant user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccounts/{id}")]
        public IActionResult FindYachtMerchantAcc(int id)
        {
            try
            {
                var result = _yachtMerchantAccService.FindYachtMerchantAccById(id);
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
        /// API Create new Yacht merchant user
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtMerchantAccounts")]
        public IActionResult CreateYachtMerchantAcc([FromBody]YachtMerchantAccCreateModel createModel)
        {
            try
            {
                var result =  _yachtMerchantAccService.CreateYachtMerchantAcc(createModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// API update Yacht merchant user
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtMerchantAccounts")]
        public IActionResult UpdateYachtMerchantAcc([FromBody]YachtMerchantAccCreateModel updateModel)
        {
            try
            {
                var result = _yachtMerchantAccService.UpdateYachtMerchantAcc(updateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }


        /// <summary>
        /// API delete Yacht merchant user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("YachtMerchantAccounts/{id}")]
        public IActionResult DeleteYachtMerchantAcc(int id)
        {
            try
            {
                var result = _yachtMerchantAccService.DeleteYachtMerchantAcc(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Get yacht merchant account by merchant id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantAccounts/ByMerchantId/{merchantId}")]
        public IActionResult GetYachtMerchantAccByMerchantId(int merchantId)
        {
            try
            {
                var result = _yachtMerchantAccService.GetYachtMerchantAccByMerchatnId(merchantId);
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