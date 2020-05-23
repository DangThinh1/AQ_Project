using AQEncrypts;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourOperationDetails;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourOperationDetailController : ControllerBase
    {
        private readonly IYachtTourOperationDetailService _yachtTourOperationDetailService;

        public YachtTourOperationDetailController(IYachtTourOperationDetailService yachtTourOperationDetailService)
        {
            _yachtTourOperationDetailService = yachtTourOperationDetailService;
        }

        [HttpPost("YachtTourOperationDetail")]
        public IActionResult CreateYachtTours([FromBody]YachtTourOperationDetailCreateModel model)
        {
            var baseresponse = _yachtTourOperationDetailService.Create(model);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourOperationDetail/YachtTourId/{yachtTourIdEncrypted}")]
        public IActionResult FindByTourId(string yachtTourIdEncrypted)
        {
            var tourId = DecryptValue(yachtTourIdEncrypted);
            var baseresponse = _yachtTourOperationDetailService.FindByTourId(tourId);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpPost("YachtTourOperationDetail/{yachtTourIdEncrypted}")]
        public IActionResult Search(string yachtTourIdEncrypted, [FromBody] YachtTourOperationDetailSearchModel searchModel)
        {
            var tourId = DecryptValue(yachtTourIdEncrypted);
            var baseresponse = _yachtTourOperationDetailService.Search(tourId, searchModel);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourOperationDetail/YachtId/{yachtIdEncrypted}")]
        public IActionResult FindByYachtId(string yachtIdEncrypted)
        {
            var yachtId = DecryptValue(yachtIdEncrypted);
            var baseresponse = _yachtTourOperationDetailService.FindByYachtId(yachtId);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtTourOperationDetail/IsActivated/{yachtIdEncrypted}/{tourIdEncrypted}/{value}")]
        public IActionResult IsActivated(string yachtIdEncrypted, string tourIdEncrypted, bool value)
        {
            var yachtId = DecryptValue(yachtIdEncrypted);
            var tourId = DecryptValue(tourIdEncrypted);
            var result = _yachtTourOperationDetailService.SetActivated(yachtId, tourId, value).IsSuccessStatusCode;
            if (result)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtTourOperationDetail/{yachtIdEncrypted}/{tourIdEncrypted}")]
        public IActionResult Delete(string yachtIdEncrypted, string tourIdEncrypted)
        {
            var yachtId = DecryptValue(yachtIdEncrypted);
            var tourId = DecryptValue(tourIdEncrypted);
            var result = _yachtTourOperationDetailService.Delete(yachtId, tourId).IsSuccessStatusCode;
            if (result)
                return Ok(result);
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