using AccommodationMerchant.Core.Models.HotelAttributes;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccommodationMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class HotelAttributesController : ControllerBase
    {
        private readonly IHotelAttributeService _hotelAttributeService;
        public HotelAttributesController(IHotelAttributeService hotelAttributeService)
        {
            _hotelAttributeService = hotelAttributeService;
        }

        [HttpPost]
        [Route("HotelAttribute")]
        public async Task<IActionResult> CreateAsync([FromBody] HotelAttributeCreateModel model)
        {
            var response = await _hotelAttributeService.CreateAsync(model);
            return Ok(response);
        }

        [HttpGet]
        [Route("HotelAttributes")]
        public IActionResult Search([FromQuery]HotelAttributeSearchModel model)
        {
            var response =  _hotelAttributeService.Search(model);
            return Ok(response);
            
        }

        [HttpGet]
        [Route("HotelAttributes/AttributeCatagory/{id}")]
        public IActionResult SearchByCategoryId(int id)
        {
            var response = _hotelAttributeService.SearchByCategoryId(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("HotelAttributes/{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            var response = await _hotelAttributeService.FindByIdAsync(id);
            return Ok(response);
           
        }

        [HttpGet]
        [Route("HotelAttributes/AttributeName/{name}")]
        public async Task<IActionResult> FindNameAsync(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                var response = await _hotelAttributeService.FindByNameAsync(name);
                return Ok(response);
            }
            return Ok(BaseResponse<HotelAttributeViewModel>.BadRequest());
        }

        [HttpPut]
        [Route("HotelAttributes")]
        public async Task<IActionResult> UpdateAsync([FromBody]HotelAttributeUpdateModel model)
        {
            var response = await  _hotelAttributeService.UpdateAsync(model);
            return Ok(response);
        }

        [HttpDelete]
        [Route("HotelAttributes/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await  _hotelAttributeService.DeleteAsync(id);
            return Ok(response);
        }



    }
}