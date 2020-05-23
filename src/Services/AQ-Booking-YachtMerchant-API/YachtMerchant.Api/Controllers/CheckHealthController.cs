using Identity.Core.Conts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class CheckHealthController : ControllerBase
    {
        private readonly ICheckHealthService _checkHealthService;
        public CheckHealthController(ICheckHealthService checkHealthService)
        {
            _checkHealthService = checkHealthService;
        }
        [HttpGet("CheckHealth/Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
        [HttpGet("CheckHealth/IsGoodHealth")]
        public IActionResult IsGoodHealth()
        {
            var result = _checkHealthService.IsGoodHealth();
            if(result.IsSuccessStatusCode)
                return Ok(result.ResponseData);
            return BadRequest();
        }
        [HttpGet("CheckHealth/ServerInfo")]
        public IActionResult ServerInfo()
        {
            var result = _checkHealthService.ServerInfo().ResponseData;
            return Ok(result);
        }
    }
}