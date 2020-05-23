using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.Yachts;
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
    public class YachtCharteringController : ControllerBase
    {
        private readonly IYachtCharteringService _yachtCharteringService;
        public YachtCharteringController(IYachtCharteringService yachtCharteringService)
        {
            _yachtCharteringService = yachtCharteringService;
        }

        [HttpGet("Yachts/YachtCharterings/CharteringDetail/charteringFId/{charteringFId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCharteringDetailByCharteringFId(string charteringFId)
        {
            var response = _yachtCharteringService.GetCharteringDetailByCharteringFId(charteringFId);
            return Ok(response);
        }
        [HttpGet("Yachts/YachtCharterings/Chartering/charteringFId/{charteringFId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCharteringByCharteringFId(string charteringFId)
        {
            var response = _yachtCharteringService.GetCharteringByCharteringFId(charteringFId);
            return Ok(response);
        }
        [HttpGet("Yachts/YachtCharterings/Chartering/uniqueFId/{uniqueId}")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetCharteringByUniqueId(string uniqueId)
        {
            var response = _yachtCharteringService.GetCharteringByUniqueId(uniqueId);
            return Ok(response);
        }
        [HttpPost]
        [Route("Yachts/YachtCharterings/UpdateStatusCharterPrivatePayment")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using Yacht/Payment page--> PAYALE , PAYMENT STRIP function.
        public IActionResult UpdateStatusCharterPrivatePayment(CharteringUpdateStatusModel charteringModel)
        {
            var result = _yachtCharteringService.UpdateStatusCharterPrivatePayment(charteringModel);
            return Ok(result);
        }
        [HttpPost]
        [Route("Yachts/YachtCharterings/GetChartering")]
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public IActionResult GetChartering(YachtCharteringRequestModel RequestModel)
        {
            var result = _yachtCharteringService.GetChartering(RequestModel);
            return Ok(result);
        }

        [HttpGet]
        [Route("Yachts/YachtCharterings/GetCharterByReservationEmail")]
        public IActionResult GetCharterByReservationEmail(string email)
        {
            var result = _yachtCharteringService.GetCharterByReservationEmail(email);
            return Ok(result);
        }
    }
}