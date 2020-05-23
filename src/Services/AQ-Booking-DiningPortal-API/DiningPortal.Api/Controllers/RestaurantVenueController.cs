using System.Linq;
using AQDiningPortal.Core.Models.Restaurants;
using AQDiningPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AQDiningPortal.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]

    public class RestaurantVenueController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IRestaurantVenueService _restaurantVenueService;
        public RestaurantVenueController(IRestaurantService restaurantService, IRestaurantVenueService restaurantVenueService)
        {
            _restaurantService = restaurantService;
            _restaurantVenueService = restaurantVenueService;
        }

        [Route("RestaurantVenueSearch")]
        [HttpGet]
        public IActionResult SearchVenue([FromQuery]RestaurantSearchModel searchModel)
        {
            var result = _restaurantService.Search(searchModel);
            if (result.ResponseData.Data.Count > 0)
            {
                string UniqueIdSerial = string.Empty;
                UniqueIdSerial = string.Join(',', result.ResponseData.Data.Select(x => x.UniqueId));
                return Ok(result);
            }
            return NotFound(result);
        }

        [Route("RestaurantVenueByResId/{resId}")]
        [HttpGet]
        public IActionResult GetVenueByRestaurantId(int resId)
        {
            var res = _restaurantVenueService.GetVenueByRestauranID(resId);
            if (res != null)
                return Ok(res);
            return NotFound();
        }
    }
}