using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [LogHelper]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtPricingPlanInfomationController : ControllerBase
    {
        private readonly IYachtPricingPlanInfomationService _pricingPlanInfomationService;

        public YachtPricingPlanInfomationController(IYachtPricingPlanInfomationService pricingPlanInfomationService)
        {
            _pricingPlanInfomationService = pricingPlanInfomationService;
        }

        [HttpGet("Yachts/YachtPricingPlanInfomations/PricingPlanInfo/yachtFId/{yachtFId}/Language/{languageId}")]
        public IActionResult GetPricingPlanInfo(string yachtFId, int languageId)
        {
            var result = _pricingPlanInfomationService.GetPricingPlanInfo(yachtFId, languageId);
            return Ok(result);
        }
    }
}