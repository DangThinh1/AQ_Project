using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
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
    public class YachtFileStreamController : ControllerBase
    {
        private readonly IYachtFileStreamService _yachtFileStreamService;

        public YachtFileStreamController(IYachtFileStreamService yachtFileStreamService)
        {
            _yachtFileStreamService = yachtFileStreamService;
        }

        [HttpGet("Yachts/YachtFileStreams/FileStream/yachtFId/{yachtFId}/categoryFId/{categoryFId}")]
        public IActionResult GetYachtFileStream(string yachtFId, int categoryFId)
        {
            var result = _yachtFileStreamService.GetFileStream(yachtFId, categoryFId);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtFileStreams/FileStream")]
        public IActionResult GetFileStreamPaging([FromQuery]YachtFileStreamSearchModel searchModel)
        {
            var result = _yachtFileStreamService.GetFileStreamPaging(searchModel);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtFileStreams/Encrypt/{yachtFId}")]
        public IActionResult Encrypt(int yachtFId)
        {
            var result = _yachtFileStreamService.Encrypt(yachtFId);
            return Ok(result);
        }
    }
}