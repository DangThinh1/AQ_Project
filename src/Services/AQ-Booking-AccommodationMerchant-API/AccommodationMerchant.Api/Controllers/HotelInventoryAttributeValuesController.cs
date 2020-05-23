using AccommodationMerchant.Core.Models.HotelAttributeValues;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
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
    public class HotelInventoryAttributeValuesController : ControllerBase
    {
        private readonly IHotelInventoryAttributeValueService _hotelInventoryAttributeValueService;

        public HotelInventoryAttributeValuesController(IHotelInventoryAttributeValueService hotelInventoryAttributeValueService)
        {
            _hotelInventoryAttributeValueService = hotelInventoryAttributeValueService;
        }

        /// <summary>
        /// Create attribute value with range 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelInventoryAttributeValues")]
        public async Task<IActionResult> CreateRangeAsync([FromBody]HotelInventoryAttributeValueCreateModel model)
        {
            if (model == null)
                return Ok(BaseResponse<bool>.BadRequest());
            string[] arrAttributeFid =  model.AttributeFid.Split(",");
            string[] arrAttributeValues = model.AttributeValue.Split(",");
               
            if(arrAttributeFid.Length > 0 && arrAttributeValues.Length>0)
            {
                var lstattributeValue = new List<HotelInventoryAttributeValueCreateModel>();
                for (int i=0; i< arrAttributeFid.Length; i++ )
                {
                    var attributeValue = new HotelInventoryAttributeValueCreateModel();
                    attributeValue.InventoryFid = model.InventoryFid;
                    attributeValue.AttributeCategoryFid = model.AttributeCategoryFid;
                    attributeValue.AttributeFid = arrAttributeFid[i];
                    attributeValue.AttributeValue = arrAttributeValues[i];
                    attributeValue.EffectiveDate = model.EffectiveDate;
                    lstattributeValue.Add(attributeValue);
                }
                if(lstattributeValue.Count > 0)
                {
                    var response = await _hotelInventoryAttributeValueService.CreateRangeAsync(lstattributeValue);
                    if (response.IsSuccessStatusCode)
                        return Ok(response);
                }
            }
            return Ok(BaseResponse<bool>.BadRequest());
        }

        /// <summary>
        /// Get all attribute of inventory By InventoryId
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelInventoryAttributeValues/{inventoryId}")]
        public IActionResult GetAllAttributeValuesByInventoryId(int inventoryId)
        {
            var response = _hotelInventoryAttributeValueService.GetAllAttributeValueByInventoryId(inventoryId);
            return Ok(response);
        }

        /// <summary>
        /// Get all attribute value of inventorty by inventoryId and category Id
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelInventoryAttributeValues/{inventoryId}/{categoryId}")]
        public IActionResult GetAllAttributeValuesByCategoryIdAndInventoryId(int inventoryId, int categoryId)
        {
            var response = _hotelInventoryAttributeValueService.GetAllAttributeValueByInventoryIdAndCategoryId(inventoryId, categoryId);
            return Ok(response);
        }

        /// <summary>
        /// Get attributes based on category to update
        /// </summary>
        /// <param name="inventoryId">ID of inventory in int format</param>
        /// <param name="categoryId">ID of attribute category in int format</param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelInventoryAttributeValueMgtUpdate/{inventoryId}/{categoryId}")]
        public IActionResult GetListUpdateAttributeValueAsync(int inventoryId, int categoryId)
        {
            var response = _hotelInventoryAttributeValueService.GetListUpdateAttributeValue(inventoryId,categoryId);
            return Ok(response);
        }


        
        /// <summary>
        /// Add attribute value
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelInventoryAttributeValue")]
        public async Task<IActionResult> AddAttributeValueAsync([FromBody]HotelInventoryAttributeValueCreateModel model)
        {
            var response =  await _hotelInventoryAttributeValueService.CreateAsync(model);
            return Ok(response);
        }

        
        /// <summary>
        /// Update attribute value  with range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("HotelInventoryAttributeValues")]
        public async Task<IActionResult> UpdateAttributeValuesRangeAsync([FromBody]HotelInventoryAttributeValueUpdateRangeModel model)
        {
            var response = await  _hotelInventoryAttributeValueService.UpdateAttributeValueRangeAsync(model);
            return Ok(response);


        }
    }
}