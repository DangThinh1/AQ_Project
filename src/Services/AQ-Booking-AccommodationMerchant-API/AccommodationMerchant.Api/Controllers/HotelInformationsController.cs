using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInformations;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelInformationsController : ControllerBase
    {
        private readonly IHotelInformationService _informationService;

        public HotelInformationsController(IHotelInformationService informationService)
        {
            _informationService = informationService;
        }

        [HttpGet("HotelInformations/{id}")]
        public IActionResult Find(int id)
        {
            if (id != 0)
            {
                var response = _informationService.Find(id);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPost("HotelInformations")]
        public IActionResult Search([FromBody]HotelInformationSearchModel model)
        {
            var response = _informationService.Search(model);
            return Ok(response);
        }

        [HttpPost("HotelInformations/{hotelFid}")]
        public IActionResult Create([FromBody]HotelInformationCreateModel model, string hotelFid)
        {
            if (!string.IsNullOrEmpty(model.HotelFid))
            {
                var response = _informationService.Create(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("HotelInformations/{id}")]
        public IActionResult Update([FromBody]HotelInformationUpdateModel model, string id)
        {
            var idDecrypted = Decrypt.DecryptToInt32(id);
            if (idDecrypted != 0)
            {
                model.Id = idDecrypted;
                var response = _informationService.Update(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("HotelInformations/{id}/IsActivated/{value}")]
        public IActionResult Activate(int id, bool value)
        {
            if (id != 0)
            {
                var response = _informationService.IsActivated(id, value);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpDelete("HotelInformations/{id}")]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var response = _informationService.Delete(id);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpGet("HotelInformations/GetInfoDetailSupported/{inforId}")]
        public IActionResult GetInfoDetailSupported(int inforId)
        {
            if (inforId != 0)
            {
                var response = _informationService.GetInfoDetailSupportedByInfoId(inforId);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }
    }
}