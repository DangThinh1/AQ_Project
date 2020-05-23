using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc;
using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInventories;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelInventoriesController : ControllerBase
    {
        private readonly IHotelInventoryService _hotelInventoryService;

        public HotelInventoriesController(IHotelInventoryService hotelInventoryService)
        {
            _hotelInventoryService = hotelInventoryService;
        }

        [HttpGet("HotelInventories/Hotel/{hotelId}/{id}")]
        public IActionResult Find(string hotelId, long id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelId);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Find(id);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }

        [HttpGet("HotelInventories")]
        public IActionResult Search([FromQuery]HotelInventorySearchModel model)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(model.HotelFid);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Search(model);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }

        [HttpPost("HotelInventories/Hotel/{hotelFid}")]
        public IActionResult Create(HotelInventoryCreateModel model, string hotelFid)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelFid);
            if (hotelIdDecrypted != 0)
            {
                model.HotelFid = hotelIdDecrypted;
                var response = _hotelInventoryService.Create(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("HotelInventories/Hotel/{hotelFid}")]
        public IActionResult Update(HotelInventoryUpdateModel model, string hotelFid)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelFid);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Update(model);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }

        [HttpDelete("HotelInventories/Hotel/{hotelFid}/{id}")]
        public IActionResult Delete(string hotelFid, long id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelFid);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Delete(id);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }

        [HttpPut("HotelInventories/Hotel/{hotelFid}/Activate/{id}")]
        public IActionResult Activate(string hotelFid, long id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelFid);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Activate(id);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }

        [HttpPut("HotelInventories/Hotel/{hotelFid}/Deactivate/{id}")]
        public IActionResult Deactivate(string hotelFid, long id)
        {
            var hotelIdDecrypted = Decrypt.DecryptToInt32(hotelFid);
            if (hotelIdDecrypted != 0)
            {
                var response = _hotelInventoryService.Deactivate(id);
                return Ok(response);
            }
            return Ok(BaseResponse<object>.BadRequest());
        }
    }
}