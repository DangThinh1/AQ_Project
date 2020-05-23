using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class YachtLandingController : ControllerBase
    {
        private readonly IYachtLandingService _yachtLandingService;
        public YachtLandingController(IYachtLandingService yachtLandingService)
        {
            _yachtLandingService = yachtLandingService;
        }

        [HttpPost]
        [Route("YachtLandings")]
        public IActionResult GetYachtByMerchantIDForLanding(SearchYachtWithMerchantIdModel searchModel)
        {
            var result = _yachtLandingService.GetYachtByMerchantIDForLanding(searchModel);
            return Ok(result);
        }
    }
}