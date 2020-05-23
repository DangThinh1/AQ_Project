using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourCharterPaymentLogsController : ControllerBase
    {
        private readonly IYachtTourCharterPaymentLogsService _yachtTourCharterPaymentLogsService;

        public YachtTourCharterPaymentLogsController(IYachtTourCharterPaymentLogsService yachtTourCharterPaymentLogsService)
        {
            _yachtTourCharterPaymentLogsService = yachtTourCharterPaymentLogsService;
        }

        /// <summary>
        /// Get log payment of charter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtTourCharterPaymentLogs/{id}")]
        public IActionResult GetYachtCharteringPaymentLogs(long id)
        {
            var result = _yachtTourCharterPaymentLogsService.GetYachtTourCharterPaymentLogsByTourCharterId(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}