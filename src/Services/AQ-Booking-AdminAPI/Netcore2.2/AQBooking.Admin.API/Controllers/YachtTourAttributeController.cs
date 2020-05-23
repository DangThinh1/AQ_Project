using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtTourAttribute;
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
    public class YachtTourAttributeController : ControllerBase
    {
        #region Fields

        private readonly IYachtTourAttributeService _yachtTourAttributeService;

        #endregion Fields

        #region Ctor
        public YachtTourAttributeController(IYachtTourAttributeService yachtTourAttributeService)
        {
            _yachtTourAttributeService = yachtTourAttributeService;
        }
        #endregion Ctor
        #region method

        [HttpGet]
        [Route("YachtTourAttributes")]
        public IActionResult SearchYachtTourAttribute([FromQuery]YachtTourAttributeSearchModel searchModel)
        {
            try
            {
                var result = _yachtTourAttributeService.SearchYachtTourAttributes(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [HttpGet]
        [Route("YachtTourAttributes/{id}")]
        public IActionResult FindYachtTourAttributes(int id)
        {
            try
            {
                var result = _yachtTourAttributeService.GetYachtTourAttributeById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("YachtTourAttributes/{id}")]
        public IActionResult DeleteYachtTourAttributes(int id)
        {
            try
            {
                var result = _yachtTourAttributeService.DeleteYachtTourAttribute(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [HttpPost]
        [Route("InsertOrUpdateYachtTourAttribute")]
        public IActionResult InsertOrUpdateYachtTourAttribute([FromBody]YachtTourAttributeCreateModel model)
        {
            try
            {
                var result = _yachtTourAttributeService.CreateOrUpdateYachtTourAttribute(model).Result;
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