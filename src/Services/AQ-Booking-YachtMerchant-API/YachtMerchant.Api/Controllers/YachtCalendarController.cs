using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtCalendarController : ControllerBase
    {
        private readonly IYachtCalendarService _calendarService;

        public YachtCalendarController(IYachtCalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet("YachtCalendar/MerchantId/{merchantId}/Year/{year}/Month/{month}")]
        public IActionResult View(int merchantId, int year, int month)
        {
            if (month >= 13)
                return BadRequest();
            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var baseresponse = _calendarService.Calendar(merchantId, start, end);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }
    }
}