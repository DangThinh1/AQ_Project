using System;
using AQEncrypts;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using YachtMerchant.Core.Models.YachtTours;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtToursController : ControllerBase
    {
        private readonly IYachtToursServices _yachtToursServices;
        public YachtToursController(IYachtToursServices yachtToursServices)
        {
            _yachtToursServices = yachtToursServices;
        }

        [HttpPost("YachtTours")]
        public IActionResult CreateYachtTours(YachtTourCreateModel model)
        {
            var user = User;
            var baseresponse = _yachtToursServices.Create(model);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTours/{encryptedId}")]
        public IActionResult FindById(string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();

            var baseresponse = _yachtToursServices.GetTourById(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTours/MerChantId/{merChantEncryptedId}")]
        public IActionResult FindByMerChantId(string merChantEncryptedId)
        {
            var merchantId = DecryptValue(merChantEncryptedId);
            if (merchantId == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.GetToursByMerchantFid(merchantId);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTours/Details/MerChantId/{merChantEncryptedId}")]
        public IActionResult FindDetailByMerChantId(string merChantEncryptedId)
        {
            var merchantId = DecryptValue(merChantEncryptedId);
            if (merchantId == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.GetTourDetailsByMerchantFid(merchantId);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpPost("YachtTours/MerChantId/{merChantEncryptedId}")]
        public IActionResult Search(string merChantEncryptedId, [FromBody]YachtTourSearchModel model)
        {
            var merchantId = DecryptValue(merChantEncryptedId);
            if (merchantId == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.Search(merchantId, model);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtToursSelectList")]
        public IActionResult GetYachtToursListSelectItem()
        {
            try
            {
                var rs = _yachtToursServices.GetYachtTourSelectItem();
                return Ok(rs);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("YachtTours/Activate/{encryptedId}")]
        public IActionResult Activate(string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.Activate(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTours/Deactivate/{encryptedId}")]
        public IActionResult Deactivate(string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.Deactivate(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpPut("YachtTours/{encryptedId}")]
        public IActionResult Update([FromBody] YachtTourUpdateModel model, string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.Update(model, id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpDelete("YachtTours/{encryptedId}")]
        public IActionResult Delete(string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.Delete(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTours/ActiveTour/{encryptedId}")]
        public IActionResult ActvieTour(string encryptedId)
        {
            var id = DecryptValue(encryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtToursServices.ActiveTour(id);
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