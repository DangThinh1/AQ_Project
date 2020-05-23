using AccommodationMerchant.Core.Models.HotelAttributes;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccommodationMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class HotelInventoryAttributesController : ControllerBase
    {
        private readonly IHotelInventoryAttributeService _hotelInventoryAttributeService;
        public HotelInventoryAttributesController(IHotelInventoryAttributeService hotelInventoryAttributeService)
        {
            _hotelInventoryAttributeService = hotelInventoryAttributeService;
        }

        [HttpPost]
        [Route("HotelInventoryAttribute")]
        public async Task<IActionResult> CreateAsync([FromBody] HotelInventoryAttributeCreateModel model)
        {
            var response = await _hotelInventoryAttributeService.CreateAsync(model);
            return Ok(response);
        }

        [HttpGet]
        [Route("HotelInventoryAttributes")]
        public IActionResult SearchAsync([FromQuery]HotelInventoryAttributeSearchModel model)
        {
            var response = _hotelInventoryAttributeService.Search(model);
            return Ok(response);
            
        }

        [HttpGet]
        [Route("HotelInventoryAttributes/AttributeCatagory/{id}")]
        public IActionResult SearchByCategoryIdAsync(int id)
        {
            var response = _hotelInventoryAttributeService.SearchByCategoryId(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("HotelInventoryAttributes/{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            var response = await _hotelInventoryAttributeService.FindByIdAsync(id);
            return Ok(response);
           
        }

        [HttpGet]
        [Route("HotelInventoryAttributes/AttributeName/{name}")]
        public async Task<IActionResult> FindNameAsync(string name)
        {
            var response = await _hotelInventoryAttributeService.FindByNameAsync(name);
            return Ok(response);
            
        }

        [HttpPut]
        [Route("HotelInventoryAttributes")]
        public async Task<IActionResult> UpdateAsync([FromBody]HotelInventoryAttributeUpdateModel model)
        {
            var response = await _hotelInventoryAttributeService.UpdateAsync(model);
            return Ok(response);
        }

        [HttpDelete]
        [Route("HotelInventoryAttributes/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _hotelInventoryAttributeService.DeleteAsync(id);
            return Ok(response);
        }



    }
}