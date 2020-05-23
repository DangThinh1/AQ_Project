using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
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
    public class YachtCharteringPaymentLogController : ControllerBase
    {
        private readonly IYachtCharteringPaymentLogService _yachtCharteringPaymentLogService;
        public YachtCharteringPaymentLogController(IYachtCharteringPaymentLogService yachtCharteringPaymentLogService)
        {
            _yachtCharteringPaymentLogService = yachtCharteringPaymentLogService;
        }
        [HttpGet]
        [Route("Yachts/YachtCharteringPaymentLog/charteringFId/{charteringFId}/statusFid/{statusFid}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCharteringPaymentLogBycharteringFId(string charteringFId, int statusFid = 1)
        {
            var response = _yachtCharteringPaymentLogService.GetCharteringPaymentLogBycharteringFId(charteringFId, statusFid);
            return Ok(response);
        }

        [HttpGet]
        [Route("Yachts/YachtCharteringPaymentLog/charteringUniqueId/{charteringUniqueId}/statusFid/{statusFid}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCharteringPaymentLogByCharteringUniqueId(string charteringUniqueId, int statusFid = 1)
        {
            var response = _yachtCharteringPaymentLogService.GetCharteringPaymentLogByCharteringUniqueId(charteringUniqueId, statusFid);
            return Ok(response);
        }
        [HttpPost]
        [Route("Yachts/YachtCharteringPaymentLog/Update")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult UpdateCharterPrivatePaymentLog(YachtCharteringPaymentLogViewModel paymentNewModel)
        {
            var response = _yachtCharteringPaymentLogService.UpdateCharterPrivatePaymentLog(paymentNewModel);
            return Ok(response);
        }
        [HttpPost]
        [Route("Yachts/YachtCharteringPaymentLog/Update/charteringUniqueId/{charteringUniqueId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult UpdateCharterPrivatePaymentLogByCharteringUniqueId(YachtCharteringPaymentLogViewModel paymentNewModel, string charteringUniqueId)
        {
            var response = _yachtCharteringPaymentLogService.UpdateCharterPrivatePaymentLogByCharteringUniqueId(paymentNewModel, charteringUniqueId);
            return Ok(response);
        }
    }
}