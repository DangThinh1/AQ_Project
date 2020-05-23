using System.Threading.Tasks;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using YachtMerchant.Core.Models.Currency;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;
        public CurrencyController(ICurrencyService services)
        {
            _service = services;
            _service.InitController(this);
        }
        /// <summary>
        /// Get currency info by currency code
        /// </summary>
        /// <param name="currencyCode">Currency code in shortname format. Ex: USD, HKD, ...</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Admin/Currency/{currencyCode}")]
        public IActionResult GetCurrencyInfoAsync(string currencyCode)
        {
            var result =  _service.GetCurrencyInfoAsync(currencyCode).Result;
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>
        /// Get culture code for currency format by currency core
        /// </summary>
        /// <param name="currencyCode">Currency code in shortname format. Ex: USD, HKD, ...</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Admin/Currency/Culture/{currencyCode}")]
        public IActionResult GetCultureCode(string currencyCode)
        {
            var result = _service.GetCultureCodeByCurrencyCode(currencyCode).Result;
            if (result == "")
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>
        /// Get currency info by location - country
        /// </summary>
        /// <param name="country">country as string format</param>   
        /// <returns></returns>
        [HttpGet]
        [Route("Admin/Currency/Country/{country}")]
        public IActionResult GetCurrencyInfoByLocation(string country)
        {
            var result = _service.GetCurrencyInfo(country.ToLower());
            if (result == null)
            {
                return NoContent();
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Get currency info by merchantId
        /// </summary>
        /// <param name="merchantId"></param>   
        /// <returns></returns>
        [HttpGet]
        [Route("Admin/Currency/GetCurrencyInfoByMerchantId/{merchantId}")]
        public IActionResult GetCurrencyInfoByMerchantId(int merchantId)
        {
            var result = _service.GetCurrencyInfo(merchantId);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

    }
}