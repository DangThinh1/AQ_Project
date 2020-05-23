using Identity.Core.Conts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AQConfigurations.Infrastructure.Services.Interfaces;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        #region Api BaseResponse Format
        [HttpGet("Countries")]
        public IActionResult GetAllCountry()
        {
            var response = _countriesService.GetAllCountries();
            return Ok(response);
        }

        [HttpGet("Countries/CountryCode/{countryCode}")]
        public IActionResult FindByCountryCode(int countryCode)
        {
            var response = _countriesService.FindByCountryCode(countryCode);
            return Ok(response);
        }
        #endregion Api BaseResponse Format

        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetAllCountryAsync()
        {
            var res = await _countriesService.GetAllCountryAsync();
            return Ok(res);
        }

        [HttpGet("GetCountryByCode")]
        public async Task<IActionResult> GetCountryByCodeAsync(int code)
        {
            var res = await _countriesService.GetCountryByCodeAsync(code);
            return Ok(res);
        }

       
    }
}