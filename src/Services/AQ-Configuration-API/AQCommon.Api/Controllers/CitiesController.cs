using Identity.Core.Conts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using AQConfigurations.Infrastructure.Services.Interfaces;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;
        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        #region Api BaseResponse Format
        [HttpGet("Cities")]
        public IActionResult GetAllCities()
        {
            var retult = _citiesService.GetAllCities();
            return Ok(retult);
        }

        [HttpGet("Cities/CountryCode/{countryCode}")]
        public IActionResult GetByCountryCode(int countryCode)
        {
            var retult = _citiesService.GetByCountryCode(countryCode);
            return Ok(retult);
        }

        [HttpGet("Cities/CityName/{cityName}")]
        public IActionResult FindByCityName(string cityName)
        {
            var retult = _citiesService.FindByCityName(cityName);
            return Ok(retult);
        }

        [HttpGet("Cities/CountryName/{countryName}")]
        public IActionResult GetByCountryName(string countryName)
        {
            var retult = _citiesService.GetByCountryName(countryName);
            return Ok(retult);
        }

        [HttpPost("Cities/CityNames")]
        public IActionResult GetByListCityNames([FromBody]List<string> cityNames)
        {
            var retult = _citiesService.GetByListCityNames(cityNames);
            return Ok(retult);
        }
        #endregion Api BaseResponse Format

        #region .
        [HttpGet("GetCity")]
        public async Task<IActionResult> GetallCityAsync()
        {
            var res = await _citiesService.GetAllCityAsync();
            return Ok(res);

        }

        [HttpGet("GetCityByCode")]
        public async Task<IActionResult> GetCityByCodeAsync(int code)
        {
            var res = await _citiesService.GetCityByCodeAsync(code);
            return Ok(res);
        }

        [HttpGet("GetCityByName")]
        public async Task<IActionResult> GetCityByNameAsync(string name)
        {
            var res = await _citiesService.GetCityByNameAsync(name);
            return Ok(res);
        }

        [HttpGet("GetCityByCountryName")]
        public async Task<IActionResult> GetCityByCountryNameAsync(string name)
        {
            var res = await _citiesService.GetCityByCountryNameAsync(name);
            return Ok(res);
        }

        [HttpPost("GetCityDistrictLstByCityLst")]
        public async Task<ActionResult> GetCityDistrictLstByCityLstAsync([FromBody]List<string> cityLst = null)
        {
            var res = await _citiesService.GetCityDistrictLstByCityLstAsync(cityLst);
            return Ok(res);
        }
        #endregion
    }
}