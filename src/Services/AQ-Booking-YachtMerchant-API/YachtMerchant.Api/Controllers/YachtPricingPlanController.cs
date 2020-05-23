using System;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtPricingPlan;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors( AQCorsPolicy.DefaultScheme)]
    public class YachtPricingPlanController : ControllerBase
    {
        private readonly IYachtPricingPlansService _yachtPricingPlansService;
        private readonly IYachtMerchantService _yachtMerchantService;
        public YachtPricingPlanController(IYachtPricingPlansService yachtPricingPlansService, IYachtMerchantService yachtMerchantService)
        {
            _yachtPricingPlansService = yachtPricingPlansService;
            _yachtMerchantService = yachtMerchantService;
        }

        [HttpPost]
        [Route("YachtPricingPlans")]
        public IActionResult CreateYachtPricingPlan(YachtPricingPlanCreateModel model)
        {
            var resultService = _yachtPricingPlansService.CreateYachtPricingPlans(model).Result;
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
                
        }
        [HttpGet]
        [Route("YachtPricingPlanSearch")]
        public IActionResult SearchYachtPricingPlan([FromQuery]YachtPricingPlanSearchModel model)
        {
            var resultService = _yachtPricingPlansService.SearchYachtPricingPlan(model);
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtPricingPlans")]
        public IActionResult DeleteYachtPricingPlan([FromQuery]int Id)
        {
            var resultService = _yachtPricingPlansService.DeleteYachtPricingPlans(Id);
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
            
        }

        [HttpPut]
        [Route("YachtPricingPlans")]
        public IActionResult UpdateYachtPricingPlan([FromBody]YachtPricingPlanCreateModel model)
        {
            var resultService = _yachtPricingPlansService.UpdateYachtPricingPlans(model);
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
           
        }
        [HttpGet]
        [Route("YachtPricingPlans/{Id}")]
        public IActionResult GetYachtPricingPlanById(int Id)
        {
            var resultService = _yachtPricingPlansService.GetById(Id);
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
           
        }
    }
}