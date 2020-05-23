using AQEncrypts;
using ExtendedUtility;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtTourPricings;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourPricingsController : ControllerBase
    {
        private readonly IYachtTourPricingService _yachtTourPricingService;

        public YachtTourPricingsController(IYachtTourPricingService yachtTourPricingService)
        {
            _yachtTourPricingService = yachtTourPricingService;
        }

        [HttpGet("YachtTourPricings/{id}")]
        public IActionResult FindById(long id)
        {
            var result = _yachtTourPricingService.GetById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet("YachtTourPricings/TourId/{tourId}/YachtId/{yachtId}")]
        public IActionResult FindByTourIdAndYacht(string tourId, string yachtId)
        {
            var tourIdDecrypted = tourId.Decrypt().ToInt32();
            var yachtIdDecrypted = yachtId.Decrypt().ToInt32();
            if (tourIdDecrypted < 1 || yachtIdDecrypted < 1)
                return BadRequest();
            var result = _yachtTourPricingService.GetByTourIdAndYachtId(tourIdDecrypted, yachtIdDecrypted);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet("YachtTourPricings/Detail/{tourId}")]
        public IActionResult FindDetailByTourId(string tourId)
        {
            var tourIdDecrypted = tourId.Decrypt().ToInt32();
            if (tourIdDecrypted < 1)
                return BadRequest();
            var result = _yachtTourPricingService.GetDetailByTourId(tourIdDecrypted);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost("YachtTourPricings/{tourId}")]
        public IActionResult Search(string tourId, [FromBody]YachtTourPricingSearchModel model)
        {
            var tourIdDecrypted = tourId.Decrypt().ToInt32();
            if (tourIdDecrypted < 1)
                return BadRequest();
            var result = _yachtTourPricingService.Search(tourIdDecrypted, model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost("YachtTourPricings/Details/{tourId}")]
        public IActionResult SearchDetail(string tourId, [FromBody]YachtTourPricingSearchModel model)
        {
            var tourIdDecrypted = tourId.Decrypt().ToInt32();
            if (tourIdDecrypted < 1)
                return BadRequest();
            var result = _yachtTourPricingService.SearchDetail(tourIdDecrypted, model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        

        [HttpDelete("YachtTourPricings/{id}")]
        public IActionResult Delete(long id)
        {
            var result = _yachtTourPricingService.Delete(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost("YachtTourPricings/TourId/{tourId}/YachtId/{yachtId}")]
        public IActionResult Create(string tourId, string yachtId, [FromBody] YachtTourPricingCreateModel model)
        {
            var tourIdDecrypted = tourId.Decrypt().ToInt32();
            var yachtIdDecrypted = yachtId.Decrypt().ToInt32();
            if (tourIdDecrypted < 1 || yachtIdDecrypted < 1)
                return BadRequest();
            var result = _yachtTourPricingService.Create(tourIdDecrypted, yachtIdDecrypted, model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut("YachtTourPricings")]
        public IActionResult Update([FromBody] YachtTourPricingUpdateModel model)
        {
            if (model.Id < 1)
                return BadRequest();
            var result = _yachtTourPricingService.Update(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}