using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInformationDetails;
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
    public class HotelInformationDetailsController : ControllerBase
    {
        private readonly IHotelInformationDetailService _detailInformationService;

        public HotelInformationDetailsController(IHotelInformationDetailService detailInformationService)
        {
            _detailInformationService = detailInformationService;
        }

        [HttpGet("HotelInformationDetails/{id}")]
        public IActionResult Find(string id)
        {
            var InformationId = Decrypt.DecryptToInt32(id);
            if (InformationId != 0)
            {
                var response = _detailInformationService.Find(InformationId);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        //[HttpPut("HotelInformationDetails")]
        //public IActionResult Search([FromBody]HotelInformationDetailSearchModel model)
        //{
        //    var response = _detailInformationService.Search(model);
        //    return Ok(response);
        //}

        [HttpPost("HotelInformationDetails")]
        public IActionResult Create([FromBody]HotelInformationDetailCreateModel model)
        {
            if (model != null)
            {
                var response = _detailInformationService.Create(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("HotelInformationDetails")]
        public IActionResult Update([FromBody]HotelInformationDetailUpdateModel model)
        {
            if (model != null)
            {
                var response = _detailInformationService.Update(model);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpPut("HotelInformationDetails/{id}/IsActivated/{value}")]
        public IActionResult Activate(string id, bool value)
        {
            var idDecrypted = Decrypt.DecryptToInt32(id);
            if (idDecrypted != 0)
            {
                var response = _detailInformationService.IsActivated(idDecrypted, value);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        [HttpDelete("HotelInformationDetails/{id}")]
        public IActionResult Delete(string id)
        {
            var idDecrypted = Decrypt.DecryptToInt32(id);
            if (idDecrypted != 0)
            {
                var response = _detailInformationService.Delete(idDecrypted);
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }
    }
}