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
    public class YachtAdditionalServiceController : ControllerBase
    {
        private readonly IYachtAdditionalService _yachtAdditionalService;

        public YachtAdditionalServiceController(IYachtAdditionalService yachtAdditionalService)
        {
            _yachtAdditionalService = yachtAdditionalService;
        }

        [HttpGet("Yachts/YachtAdditionalServices/AddictionalPackages/yachtFId/{yachtFId}")]
        public IActionResult GetYachtAddictionalPackageByYachtId(string yachtFId)
        {
            var result = _yachtAdditionalService.GetYachtAddictionalPackageByYachtId(yachtFId);
            return Ok(result);
        }
    }
}