using System.Linq;
using APIHelpers.Response;
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
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [Route("Restaurants")]
        public IActionResult Search([FromQuery]RestaurantSearchModel searchModel)
        {
            //LogHelper.InsertLog()
            var result = _restaurantService.Search(searchModel);
            if (result.ResponseData.Data.Count > 0)
            {
                string UniqueIdSerial = string.Empty;
                UniqueIdSerial = string.Join(',', result.ResponseData.Data.Select(x => x.UniqueId));
                //LogHelper.InsertLog("RestaurantsController", "SearchCountRestaurant",LogType.Info , UniqueIdSerial, "Writing for counting found out seaching of retaurant");
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("Restaurants/ComboBinding/City/{City}/Zone/{Zone}")]
        public IActionResult GetComboBindingByCityAndZone(string City, string Zone)
        {
            var result = _restaurantService.GetComboBindingByCityAndZone(City, Zone);
            return Ok(result);
        }

        /// <summary>
        /// Get Detail properties of restaurant by id and filter by language
        /// </summary>
        /// <param name="restaurantId">Id of restaurant in int format</param>
        /// <param name="languageId">Id of language in int format by default 1</param>
        /// <returns>Restaurant Detail view model content all info of restaurant</returns>
        [HttpGet("Restaurants/{restaurantId}/Language/{languageId}")]
        public IActionResult GetDetail(int restaurantId, int languageId = 1)
        {
            var response = _restaurantService.GetDetail(restaurantId, languageId);
            return Ok(response);
        }

        /// <summary>
        /// Get Detail properties of restaurant by id and filter by language
        /// => the same GetDetail Api but support filter Activated Date instead Now Date
        /// </summary>
        /// <param name="requestModel">Model of request Get Details Restaurant</param>

        [HttpPost("Restaurants/Details")]
        public IActionResult GetDetailWithActivatedDate([FromBody] RestaurantDetaiRequestModel requestModel)
        {
            if (requestModel == null)
            {
                return Ok(BaseResponse<RestaurantDetailViewModel>.BadRequest());
            }
            var response = _restaurantService.GetDetail(requestModel.RestaurantId, requestModel.LanguageId, requestModel.ActivatedDate);
            return Ok(response);
        }

        [HttpGet("Restaurants/RestaurantPartners/DisplayNumber/{DisplayNumber}/ImageType/{ImageType}")]
        public IActionResult GetRestaurantPartners(int DisplayNumber, int ImageType)
        {
            var response = _restaurantService.GetRestaurantPartners(DisplayNumber, ImageType);
            return Ok(response);
        }
        [HttpGet("Restaurants/RestaurantByMerchantId")]
        public IActionResult GetRestaurantsByMerchantFId([FromQuery] SearchRestaurantWithMerchantIdModel searchModel)
        {
            var response = _restaurantService.GetRestaurantsByMerchantFId(searchModel);
            return Ok(response);
        }
    }
}