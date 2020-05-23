using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtMerchantFileStream;
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
    public class YachtMerchantFileStreamController : ControllerBase
    {
        #region Fields
        private readonly IYachtMerchantFileStreamService _yachtMerchantFileStreamService;
        #endregion

        #region Ctor
        public YachtMerchantFileStreamController(IYachtMerchantFileStreamService yachtMerchantFileStreamService)
        {
            _yachtMerchantFileStreamService = yachtMerchantFileStreamService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("YachtMerchantFileStream")]
        public IActionResult GetAllYachtMerchantFileStream()
        {
            try
            {
                var result = _yachtMerchantFileStreamService.GetAllYachtMerchantFileStream();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("YachtMerchantFileStream/{id}")]
        public IActionResult GetYachtMerchantFileStreamById(int id)
        {
            try
            {
                var result = _yachtMerchantFileStreamService.GetYachtMerchantFileStreamById(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("YachtMerchantFileStream/Type/{typeId}")]
        public IActionResult GetYachtMerchantFileStreamByType(int typeId)
        {
            try
            {
                var result = _yachtMerchantFileStreamService.GetYachtMerchantFileStreamByType(typeId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        [Route("YachtMerchantFileStream")]
        public IActionResult CreateYachtMerchantFileStream(YachtMerchantFileStreamCreateModel model)
        {
            try
            {
                var result = _yachtMerchantFileStreamService.CreateYachtMerchantFileStream(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("YachtMerchantFileStream")]
        public IActionResult UpdateYachtMerchantFileStream(YachtMerchantFileStreamUpdateModel model)
        {
            try
            {
                var result = _yachtMerchantFileStreamService.UpdateYachtMerchantFileStream(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("YachtMerchantFileStream/{id}")]
        public IActionResult DeleteYachtMerchantFileStream(int id)
        {
            try
            {
                var result = _yachtMerchantFileStreamService.DeleteYachtMerchantFileStream(id);
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