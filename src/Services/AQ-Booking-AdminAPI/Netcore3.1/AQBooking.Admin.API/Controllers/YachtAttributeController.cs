using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.YachtAttribute;
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
    public class YachtAttributeController : ControllerBase
    {
        #region Fields

        private readonly IYachtAttributeService _yachtAttributeService;

        #endregion Fields

        #region Ctor
        public YachtAttributeController(IYachtAttributeService yachtAttributeService)
        {
            _yachtAttributeService = yachtAttributeService;
        }
        #endregion Ctor
        #region method

        [HttpGet]
        [Route("YachtAttributes")]
        public IActionResult SearchYachtAttribute([FromQuery]YachtAttributeSearchModel searchModel)
        {
            try
            {
                var result = _yachtAttributeService.SearchYachtAttributes(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [HttpGet]
        [Route("YachtAttributes/{id}")]
        public IActionResult FindYachtAttributes(int id)
        {
            try
            {
                var result = _yachtAttributeService.GetYachtAttributeById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("YachtAttributes/{id}")]
        public IActionResult DeleteYachtTourAttributes(int id)
        {
            try
            {
                var result = _yachtAttributeService.DeleteYachtAttribute(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [HttpPost]
        [Route("InsertOrUpdateYachtAttribute")]
        public IActionResult InsertOrUpdateYachtAttribute([FromBody]YachtAttributeCreateModel model)
        {
            try
            {
                var result = _yachtAttributeService.CreateOrUpdateYachtAttribute(model).Result;
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