using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelAttributeValues;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using ExtendedUtility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class HotelAttributeValuesController : ControllerBase
    {
        private readonly IHotelAttributeValueService _hotelAttributeValueService;

        public HotelAttributeValuesController(IHotelAttributeValueService hotelAttributeValueService)
        {
            _hotelAttributeValueService = hotelAttributeValueService;
        }
        /// <summary>
        /// Add attribute 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelAttributeValue")]
        public async Task<IActionResult> AddAttributeValueAsync([FromBody]HotelAttributeValueCreateModel model)
        {
            var response = await _hotelAttributeValueService.CreateAsync(model);
            return Ok(response);
        }

        /// <summary>
        /// Create attribute with range 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelAttributeValues")]
        public async Task<IActionResult> CreateRangeAsync([FromBody]HotelAttributeValueCreateModel model)
        {
            string[] arrAttributeFid =  model.AttributeFid.Split(",");
            string[] arrAttributeValues = model.AttributeValue.Split(",");
               
            if(arrAttributeFid.Length > 0 && arrAttributeValues.Length>0)
            {
                var lstattributeValue = new List<HotelAttributeValueCreateModels>();
                for (int i=0; i< arrAttributeFid.Length; i++ )
                {
                    var attributeValue = new HotelAttributeValueCreateModels();
                    attributeValue.HotelFid = model.HotelFid;
                    attributeValue.AttributeCategoryFid = model.AttributeCategoryFid;
                    attributeValue.AttributeFid = arrAttributeFid[i].ToInt32();
                    attributeValue.AttributeValue = arrAttributeValues[i];
                    attributeValue.EffectiveDate = model.EffectiveDate;
                    lstattributeValue.Add(attributeValue);
                }
                if(lstattributeValue.Count > 0)
                {
                    var response =  await _hotelAttributeValueService.CreateRangeAsync(lstattributeValue);
                    if (response.IsSuccessStatusCode)
                        return Ok(response);
                }
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        /// <summary>
        /// Get all attribute Of Hotel By HotelId 
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelAttributeValues/{hotelId}")]
        public IActionResult GetHotelAttributeValuesByHotelId(string hotelId)
        {
            var hotelIdDecryped = hotelId.DecryptToInt32();
            if(hotelIdDecryped != 0)
            {
                var response = _hotelAttributeValueService.GetAllAttributeValueByHotelId(hotelIdDecryped);
                return Ok(response);
            }
            
            return Ok(BaseResponse<List<HotelAttributeValueViewModel>>.BadRequest());
        }

        /// <summary>
        /// Get all attribute of Hotel By Category Id
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelAttributeValues/{hotelId}/{categoryId}")]
        public IActionResult GetAttributeValuesByCategoryId(string hotelId, int categoryId)
        {
            var hotelIdDecryped = hotelId.DecryptToInt32();
            if (hotelIdDecryped != 0) {
                var response = _hotelAttributeValueService.GetAllAttributeValueByHotelIdAndCategoryId(hotelIdDecryped, categoryId);
                return Ok(response);
            }
            return Ok(BaseResponse<List<HotelAttributeValueViewModel>>.BadRequest());
        }

        /// <summary>
        /// Get Hotel attributes based on category to update
        /// </summary>
        /// <param name="hotelId">ID of Hotel in int format</param>
        /// <param name="categoryId">ID of attribute category in int format</param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelAttributeValueMgtUpdate/{hotelId}/{categoryId}")]
        public IActionResult GetListUpdateAttributeValue(string hotelId, int categoryId)
        {
            var hotelIdDecryped = hotelId.DecryptToInt32();
            if (hotelIdDecryped != 0)
            {
                var response = _hotelAttributeValueService.GetListUpdateAttributeValue(hotelIdDecryped, categoryId);
                return Ok(response);
            }

            return Ok(BaseResponse<List<HotelAttributeValueMgtUpdateModel>>.BadRequest());
        }

        /// <summary>
        /// Update attribute values
        /// </summary>
        /// <param name="model">Update model</param>
        /// <param name="hotelId">Hotel id</param>
        /// <returns>Success or not</returns>
        [HttpPut]
        [Route("HotelAttributeValues/{hotelId}")]
        public IActionResult UpdateAttributeValuesRangeAsync([FromBody]HotelAttributeValueUpdateRangeModel model, string hotelId)
        {
            var hotelIdDecryped = hotelId.DecryptToInt32();
            if (hotelIdDecryped != 0)
            {
                model.HotelFid = hotelIdDecryped;
                var response = _hotelAttributeValueService.UpdateAttributeValueRangeAsync(model).Result;
                return Ok(response);
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }
    }
}