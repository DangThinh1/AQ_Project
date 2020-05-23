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
    public class YachtInformationDetailController : ControllerBase
    {
        private readonly IYachtInformationDetailService _yachtInformationDetailService;

        public YachtInformationDetailController(IYachtInformationDetailService yachtInformationDetailService)
        {
            _yachtInformationDetailService = yachtInformationDetailService;
        }

        [HttpGet("Yachts/YachtInformationDetails/InfomationDetail/yachtFId/{yachtFId}/Language/{lang}")]
        public IActionResult GetInfomationDetailByYachtFId(string yachtFId, int lang)
        {
            var result = _yachtInformationDetailService.GetInfomationDetailByYachtFId(yachtFId, lang);
            return Ok(result);
        }
    }
}