using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc;
using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.Hotels;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using AQEncrypts;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Get hotel basic profile with Hotel Id
        /// </summary>
        /// <param name="hotelId">yachtId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Hotels/GetBasicProfile/{hotelId}")]
        public async Task<IActionResult> GetBasicProfile(int hotelId)
        {
            var response = await _hotelService.GetHotelBasicProfile(hotelId);
            return Ok(response);
        }

       

        [HttpGet("Hotels/{id}")]
        public IActionResult Find(string id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(id);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelService.Find(hotelIdDecrypted);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("Hotels")]
        public IActionResult Search([FromBody]HotelSearchModel model)
        {
            var response = _hotelService.Search(model);
            return Ok(response);
        }

        [HttpPost("Hotels/merchantId/{merchantId}")]
        public IActionResult Create([FromBody]HotelCreateModel model, string merchantId)
        {
            var merchantIdDecrypted = Decrypt.DecryptToInt32(merchantId);
            if(merchantIdDecrypted != 0)
            {
                model.MerchantFid = merchantIdDecrypted;
                var response = _hotelService.Setup(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("Hotels/{id}")]
        public IActionResult Update([FromBody]HotelUpdateModel model, string id)
        {
            var hotelIdDecrypted = Terminator.DecryptToInt32(id);
            if (hotelIdDecrypted != 0)
            {
                model.Id = hotelIdDecrypted;
                var response = _hotelService.Update(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("Hotels/{id}/ActiveForOperation/{value}")]
        public IActionResult ActiveForOperation(string id, bool value)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(id);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelService.ActiveForOperation(hotelIdDecrypted, value);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpDelete("Hotels/{id}")]
        public IActionResult Delete(string id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(id);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelService.Delete(hotelIdDecrypted);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }
    }
}