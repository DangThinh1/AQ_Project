using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Conts;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtPortController : ControllerBase
    {
        #region Fields
        private readonly IYachtPortService _yachtPortLocationService;
        #endregion

        #region Ctor
        public YachtPortController(IYachtPortService yachtPortLocationService)
        {
            this._yachtPortLocationService = yachtPortLocationService;
        }
        #endregion

        #region Methods
        #endregion
        [HttpGet]
        [Route("YachtPort/GetListPortLoaction")]
        public IActionResult GetListPortLocation()
        {
            var result = _yachtPortLocationService.GetListPortLocation();
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpGet]
        [Route("YachtPort/CityName/{cityName}")]
        public IActionResult GetPortLocationByCityName(string cityName)
        {
            var result = _yachtPortLocationService.GetPortLocationByCityName(cityName);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtPort/Country/{name}")]
        public IActionResult GetPortLocationByCountry(string name)
        {
            var result = _yachtPortLocationService.GetPortLocationByCountry(name);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
           
        }
    }
}