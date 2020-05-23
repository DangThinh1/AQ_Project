using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtMerchant;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtLandingPageController : ControllerBase
    {
        private readonly IYachtMerchantService _yachtMerchantService;
        public YachtLandingPageController(IYachtMerchantService yachtMerchantService)
        {
            _yachtMerchantService = yachtMerchantService;
        }
        [HttpPut]
        [Route("YachtLandingPageOption")]
        public IActionResult UpdateLandingPageOption(YachtMerchantViewModel model)
        {
            var resultService = _yachtMerchantService.UpdateLandingPage(model);
            if (resultService.IsSuccessStatusCode)
                return Ok(resultService);
            return BadRequest();
        }
    }
}