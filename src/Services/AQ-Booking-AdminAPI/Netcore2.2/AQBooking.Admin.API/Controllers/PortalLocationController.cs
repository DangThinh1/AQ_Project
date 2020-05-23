using AQBooking.Admin.Core.Models.PortalLocation;
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
    public class PortalLocationController : ControllerBase
    {
        #region Fields

        private readonly IPortalLocationService _portalLocationService;

        #endregion Fields

        #region Ctor

        public PortalLocationController(IPortalLocationService portalLocationService)
        {
            _portalLocationService = portalLocationService;
        }

        #endregion Ctor

        #region Methods

        [HttpGet]
        [Route("PortalLocation")]
        public IActionResult SearchPortalLocation([FromQuery]PortalLocationSearchModel searchModel)
        {
            try
            {
                var result = _portalLocationService.SearchPortalLocation(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("PortalLocation/{id}")]
        public IActionResult FindPortalLocation(int id)
        {
            try
            {
                var result = _portalLocationService.GetPortalLocationById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        [Route("PortalLocation")]
        public IActionResult CreatePortalLocation(PortalLocationCreateModel model)
        {
            try
            {
                var result = _portalLocationService.CreatePortalLocation(model).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("PortalLocation")]
        public IActionResult UpdatePortalLocation(PortalLocationCreateModel model)
        {
            try
            {
                var result = _portalLocationService.UpdatePortalLocation(model).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("PortalLocation/{id}")]
        public IActionResult DeletePortalLocation(int id)
        {
            try
            {
                var result = _portalLocationService.DeletePortalLocation(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("PortalLocation/{id}")]
        public IActionResult DisablePortalLocation(int id)
        {
            try
            {
                var result = _portalLocationService.DisablePortalLocation(id).Result;
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