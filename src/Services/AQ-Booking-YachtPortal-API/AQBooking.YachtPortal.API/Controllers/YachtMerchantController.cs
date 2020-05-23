using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIHelpers.Response;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtMerchantController : ControllerBase
    {
        private readonly IYachtMerchantService _yachtMerchantService;
        public YachtMerchantController(IYachtMerchantService yachtMerchantService)
        {
            _yachtMerchantService = yachtMerchantService;
        }

        [HttpGet("Yachts/YachtMerchants/DisplayNumber/{DisplayNumber}/ImageType/{ImageType}")]
        public IActionResult GetYachtMerchantsByDisplayNumber(int DisplayNumber, int ImageType)
        {
            var response = _yachtMerchantService.GetYachtMerchantsByDisplayNumber(DisplayNumber, ImageType);
            return Ok(response);
        }
        [HttpGet("Yachts/YachtMerchants/MechantById/{Id}")]
        public IActionResult GetYachtMerchantsById(string Id)
        {
            var response = _yachtMerchantService.GetYachtMerchantsById(Id);
            return Ok(response);
        }

        [HttpGet("YachtMerchants/YachtMerchantFileStreams/{merchantId}")]
        public IActionResult GetYachtMerchantLogoByMerchantId(int merchantId)
        {
            var response = new BaseResponse<int>();
            var result = _yachtMerchantService.GetMerchantLogoByMerchantId(merchantId);
            if (result != 0)
                response = BaseResponse<int>.Success(result);
            else
                response = BaseResponse<int>.BadRequest(0, "File not found", "FILE_NOT_FOUND", "");

            return Ok(response);
        }
    }
}