using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourNonOperationDays;
using AQEncrypts;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourNonOperationDaysController : ControllerBase
    {
        private readonly IYachtTourNonOperationDayService _yachtTourNonOperationDayService;

        public YachtTourNonOperationDaysController(IYachtTourNonOperationDayService yachtTourNonOperationDayService)
        {
            _yachtTourNonOperationDayService = yachtTourNonOperationDayService;
        }

        [HttpPost("YachtTourNonOperationDays")]
        public IActionResult CreateYachtTours([FromBody]List<YachtTourNonOperationDayCreateModel> model)
        {
            var baseresponse = _yachtTourNonOperationDayService.Create(model);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourNonOperationDays/{id}")]
        public IActionResult FindById(int id)
        {
            var baseresponse = _yachtTourNonOperationDayService.FindById(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourNonOperationDays/YachtTourId")]
        public IActionResult FindByTourId([FromQuery] YachtTourNonOperationDaySearchModel searchModel)
        {
            var tourId = DecryptValue(searchModel.yachtTourIdEncrypted);
            var baseresponse = _yachtTourNonOperationDayService.FindByTourFid(tourId, searchModel);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourNonOperationDays/YachtId/{yachtIdEncrypted}")]
        public IActionResult FindByYachtId(string yachtIdEncrypted)
        {
            var yachtId = DecryptValue(yachtIdEncrypted);
            var baseresponse = _yachtTourNonOperationDayService.FindByYachtFid(yachtId);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpDelete("YachtTourNonOperationDays/{id}")]
        public IActionResult Delete(int id)
        {
            var baseresponse = _yachtTourNonOperationDayService.Delete(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        private int DecryptValue(string encryptedString)
        {
            try
            {
                var decryptedValue = Terminator.Decrypt(encryptedString);
                var intValue = int.Parse(decryptedValue);
                return intValue;
            }
            catch
            {
                return 0;
            }
        }
    }
}