using Identity.Core.Conts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtTourAttributesController : ControllerBase
    {
        private readonly IYachtTourAttributeService _yachtTourAttributeService;

        public YachtTourAttributesController(IYachtTourAttributeService yachtTourAttributeService)
        {
            _yachtTourAttributeService = yachtTourAttributeService;
        }

        [HttpGet("YachtTourAttributes")]
        public IActionResult GetAll()
        {
            var result = _yachtTourAttributeService.All();
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}