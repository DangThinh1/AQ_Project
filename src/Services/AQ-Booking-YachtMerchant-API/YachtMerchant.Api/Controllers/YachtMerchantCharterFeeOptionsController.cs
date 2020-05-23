using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtMerchantCharterFeeOptionsController : ControllerBase
    {
        private readonly IYachtMerchantCharterFeeOptionsService _yachtMerchantCharterFeeOptionsService;
        public YachtMerchantCharterFeeOptionsController(IYachtMerchantCharterFeeOptionsService yachtMerchantCharterFeeOptionsService)
        {
            _yachtMerchantCharterFeeOptionsService = yachtMerchantCharterFeeOptionsService;
        }

        /// <summary>
        /// Get all charter fee options of merchant by MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtMerchantCharterFeeOptions/{id}")]
        public IActionResult GetAllYachtMerchantCharterFeeOptionsByMerchantId(int id)
        {
            var result = _yachtMerchantCharterFeeOptionsService.GetAllCharterFeeOptionOfMerchantByMerchantId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }

    
}