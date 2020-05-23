using AQEncrypts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using YachtMerchant.Core.Models.YachtTourCounters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourCountersController : ControllerBase
    {
        private readonly IYachtTourCounterServices _yachtTourCounterServices;

        public YachtTourCountersController(IYachtTourCounterServices yachtTourCounterServices)
        {
            _yachtTourCounterServices = yachtTourCounterServices;
        }

        [HttpGet]
        [Route("YachtTourCounters/{idEncripted}")]
        public IActionResult Detail(string idEncripted)
        {
            var id = DecryptValue(idEncripted);
            if(id == 0)
                return BadRequest();
            var result = _yachtTourCounterServices.FindById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtTourCounters")]
        public IActionResult Create(YachtTourCounterCreateModel model)
        {
            var result = _yachtTourCounterServices.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtTourCounters")]
        public IActionResult Update(YachtTourCounterViewModel model)
        {
            var result = _yachtTourCounterServices.Update(model);
            if (result.IsSuccessStatusCode)
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