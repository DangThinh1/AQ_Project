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
    public class YachtAttributeValueController : ControllerBase
    {
        private readonly IYachtAttributevalueService _yachtAttributevalueService;

        public YachtAttributeValueController(IYachtAttributevalueService yachtAttributevalueService)
        {
            _yachtAttributevalueService = yachtAttributevalueService;
        }

        [HttpGet("Yachts/YachtAttributevalues/YachtAttributesCharterPrivate/yachtFId/{yachtFId}/categoryFId/{categoryFId}/isInclude/{isInclude}")]
        public IActionResult GetYachtAttributesCharterPrivate(string yachtFId, int categoryFId, bool isInclude, [FromQuery] List<string> attributeName)
        {
            var result = _yachtAttributevalueService.GetAttributesCharterPrivate(yachtFId, categoryFId, isInclude, attributeName);
            return Ok(result);
        }
        [HttpGet("Yachts/YachtAttributevalues/GetAttributesCharterPrivateGeneral/yachtFId/{yachtFId}/categoryFId/{categoryFId}/isInclude/{isInclude}")]
        public IActionResult GetAttributesCharterPrivateGeneral(string yachtFId, int categoryFId, bool isInclude, [FromQuery] List<string> attributeName)
        {
            var result = _yachtAttributevalueService.GetAttributesCharterPrivateGeneral(yachtFId, categoryFId, isInclude, attributeName);
            return Ok(result);
        }

        [HttpGet("Yachts/YachtAttributevalues/GetAttributesCharterPrivateGeneral2/yachtFId/{yachtFId}/categoryFId/{categoryFId}/isInclude/{isInclude}")]
        public IActionResult GetAttributesCharterPrivateGeneral2(string yachtFId, int categoryFId, bool isInclude, [FromQuery] List<string> attributeName)
        {
            var result = _yachtAttributevalueService.GetAttributesCharterPrivateGeneral2(yachtFId, categoryFId, isInclude, attributeName);
            return Ok(result);
        }
    }
}