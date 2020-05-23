using Microsoft.AspNetCore.Mvc;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace AQConfigurations.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currency;

        public CurrenciesController(ICurrencyService currency)
        {
            _currency = currency;
        }

        [HttpGet]
        [Route("Currencies/CountryName/{countryName}")]
        public IActionResult GetCurrencyByCountryName(string countryName)
        {
            var result = _currency.FindByCountryName(countryName);
            return Ok(result);
        }

        [HttpGet]
        [Route("Currencies/{currencyCode}")]
        public IActionResult GetCurrencyBycountryName(string currencyCode)
        {
            var result = _currency.Find(currencyCode);
            return Ok(result);
        }

        [HttpGet]
        [Route("Currencies")]
        public IActionResult All()
        {
            var result = _currency.All();
            return Ok(result);
        }
    }
}