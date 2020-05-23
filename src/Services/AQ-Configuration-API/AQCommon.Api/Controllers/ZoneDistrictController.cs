using Identity.Core.Conts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Infrastructure.Services.Interfaces;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class ZoneDistrictController : ControllerBase
    {
        private readonly IZoneDistrictService _zoneDistrictService;
        public ZoneDistrictController(IZoneDistrictService zoneDistrictService)
        {
            _zoneDistrictService = zoneDistrictService;
        }

        #region Api BaseResponse Format
        [HttpGet("ZoneDistricts")]
        public IActionResult GetallStates()
        {
            var res = _zoneDistrictService.GetAllStates();
            return Ok(res);
        }

        [HttpGet("ZoneDistricts/CityCode/{cityCode}")]
        public IActionResult GetStatesByCityCode(int cityCode)
        {
            var result = _zoneDistrictService.GetStatesByCityCode(cityCode);
            return Ok(result);
        }

        [HttpGet("ZoneDistricts/ZoneDistrictName/{zoneDistrictName}")]
        public IActionResult GetStatesByZoneDistrictName(string zoneDistrictName)
        {
            var result = _zoneDistrictService.GetStatesByZoneDistrictName(zoneDistrictName);
            return Ok(result);
        }
        #endregion Api BaseResponse Format
        //***********************************************************************//
        [HttpGet("ZoneDistrict/GetZonedistrict")]
        public async Task<IActionResult> GetallStateAsync()
        {
            var res = await _zoneDistrictService.GetAllStatesAsync();
            return Ok(res);
        }

        [HttpGet("ZoneDistrict/GetZonedistrictByCityCode")]
        public async Task<IActionResult> GetStateByCityAsync(int cityCode)
        {
            var result = await _zoneDistrictService.GetStatesByCityAsync(cityCode);
            return Ok(result);
        }

        [HttpGet("ZoneDistrict/GetZonedistrictByCityName")]
        public async Task<ActionResult<List<StateViewModel>>> GetStateByCityNameAsync(string cityName)
        {
            var result = await _zoneDistrictService.GetStatesByCityNameAsync(cityName);
            return Ok(result);
        }
    }
}