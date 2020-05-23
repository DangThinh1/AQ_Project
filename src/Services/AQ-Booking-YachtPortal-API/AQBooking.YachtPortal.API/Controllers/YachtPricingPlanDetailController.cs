using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Infrastructure.Helpers;
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
    public class YachtPricingPlanDetailController : ControllerBase
    {
        private readonly IYachtPricingPlanDetailService _yachtPricingPlanDetailService;

        public YachtPricingPlanDetailController(IYachtPricingPlanDetailService yachtPricingPlanDetailService)
        {
            _yachtPricingPlanDetailService = yachtPricingPlanDetailService;
        }

        [HttpGet("Yachts/YachtPricingPlanDetails/PricingPlanDetail/yachtFId/{yachtFId}")]
        public IActionResult GetPricingPlanDetailByYachtFId(string yachtFId)
        {
            var result = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtFId);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtPricingPlanDetails/PricingPlanDetail/yachtFId/{yachtFId}/pricingTypeFId/{pricingTypeFId}")]
        public IActionResult GetPricingPlanDetailYachtFIdAndPricingTypeFId(string yachtFId, int pricingTypeFId = 0)
        {
            //DebugHelper.LogBug("GetPricingPlanDetailYachtFIdAndPricingTypeFId", $"Request: yachtFId = {yachtFId}, pricingTypeFId = {pricingTypeFId}");
            var result = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFIdAndPricingTypeFId(yachtFId, pricingTypeFId);
            return Ok(result);
        }
    }
}