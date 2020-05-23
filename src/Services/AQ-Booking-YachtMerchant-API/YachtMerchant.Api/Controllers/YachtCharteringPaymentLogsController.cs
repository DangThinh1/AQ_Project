using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtCharteringPaymentLogsController : ControllerBase
    {
        private readonly IYachtCharteringPaymentLogsService _yachtCharteringPaymentLogsService;
        public YachtCharteringPaymentLogsController(IYachtCharteringPaymentLogsService yachtCharteringPaymentLogsService)
        {
            _yachtCharteringPaymentLogsService = yachtCharteringPaymentLogsService;
        }


        /// <summary>
        /// Get log payment of chartering
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtCharteringPaymentLogs/{id}")]
        public IActionResult GetYachtCharteringPaymentLogs(long id)
        {
            var result = _yachtCharteringPaymentLogsService.GetYachtCharteringPaymentLogsByCharteringId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}