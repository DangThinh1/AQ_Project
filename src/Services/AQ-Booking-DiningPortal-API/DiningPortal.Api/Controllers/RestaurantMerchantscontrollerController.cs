using AQDiningPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AQDiningPortal.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class RestaurantMerchantscontroller : Controller
    {
        private readonly IRestaurantMerchantService _restaurantMerchantService;
        public RestaurantMerchantscontroller(IRestaurantMerchantService restaurantMerchantService)
        {
            _restaurantMerchantService = restaurantMerchantService;
        }

        [HttpGet("RestaurantMerchants/DisplayNumber/{DisplayNumber}/ImageType/{ImageType}")]
        public IActionResult GetMerchantsByDisplayNumber(int DisplayNumber, int ImageType)
        {
            var response = _restaurantMerchantService.GetResMerchantsByDisplayNumber(DisplayNumber, ImageType);
            return Ok(response);
        }
        [HttpGet("RestaurantMerchants/MechantById/{Id}")]
        public IActionResult GetResMerchantsById(string Id)
        {
            var response = _restaurantMerchantService.GetResMerchantsById(Id);
            return Ok(response);
        }
    }
}