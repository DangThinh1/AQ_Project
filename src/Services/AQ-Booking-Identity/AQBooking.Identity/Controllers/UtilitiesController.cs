using Microsoft.AspNetCore.Mvc;
using Identity.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        [HttpGet("CheckHealth")]
        public IActionResult CheckHealth()
        {
            return Ok();
        }

        [HttpGet("random")]
        [AllowAnonymous]
        public IActionResult random()
        {
            var randomStr = UniqueIDHelper.GenerateString(12);

            return Ok();
        }
    }
}