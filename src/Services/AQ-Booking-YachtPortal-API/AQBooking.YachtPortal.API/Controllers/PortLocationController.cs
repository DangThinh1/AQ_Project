using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class PortLocationController : ControllerBase
    {
        private readonly IPortLocationService _portLocationService;
        private readonly IYachtService _yachtService;

        public PortLocationController(IYachtService yachtService, IPortLocationService portLocationService)
        {
            _yachtService = yachtService;
            _portLocationService = portLocationService;
        }

        [HttpGet("PortLocations/CityNames")]
        public IActionResult PortLocation([FromQuery]List<string> cityNames)
        {
            var result = _portLocationService.GetPortLocationByCity(cityNames);
            return Ok(result);
        }
        [HttpGet("PortLocations/ByCountry/{countryName}")]
        public IActionResult PortLocationByCountry(string countryName)
        {
            var result = _portLocationService.GetPortLocationByCountry(countryName);
            return Ok(result);
        }
    }
}