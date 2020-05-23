using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtAttributeValues;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtAttributeValueController : ControllerBase
    {
        private readonly IYachtAttributeValueService _yachtAttributeValueService;

        public YachtAttributeValueController(IYachtAttributeValueService yachtAttributeValueService)
        {
            _yachtAttributeValueService = yachtAttributeValueService;
        }

        /// <summary>
        /// Create attribute with range 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtAttributeValues")]
        public IActionResult CreateRangeAsync([FromBody] YachtAttributeValuesCreateModel model)
        {
            
            string[] arrAttributeFid =  model.AttributeFid.Split(",");
            string[] arrAttributeValues = model.AttributeValue.Split(",");
               
            if(arrAttributeFid.Length > 0 && arrAttributeValues.Length>0)
            {
                var lstattributeValue = new List<YachtAttributeValuesCreateModel>();
                for (int i=0; i< arrAttributeFid.Length; i++ )
                {
                    var attributeValue = new YachtAttributeValuesCreateModel();
                    attributeValue.YachtFid = model.YachtFid;
                    attributeValue.AttributeCategoryFid = model.AttributeCategoryFid;
                    attributeValue.AttributeFid = arrAttributeFid[i];
                    attributeValue.AttributeValue = arrAttributeValues[i];
                    attributeValue.EffectiveDate = model.EffectiveDate;
                    lstattributeValue.Add(attributeValue);
                }
                if(lstattributeValue.Count > 0)
                {
                    var result =  _yachtAttributeValueService.CreateRangeAsync(lstattributeValue).Result;
                    if (result.IsSuccessStatusCode)
                        return Ok(result);
                }
            }
            return BadRequest();
            
        }

        /// <summary>
        /// Get all attribute Of Yacht  By YachtId 
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtAttributeValues/{yachtId}")]
        public IActionResult GetYachtAttributeValuesByYachtId(int yachtId)
        {
            var result = _yachtAttributeValueService.GetAllAttributeValueByYachtIdAsync(yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
            
        }

        /// <summary>
        /// Get all attribute of Yacht  By Category Id
        /// </summary>
        /// <param name="yachtId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtAttributeValues/{yachtId}/{categoryId}")]
        public IActionResult GetYachtAttributeValuesByCategoryId(int yachtId, int categoryId)
        {
            var result = _yachtAttributeValueService.GetAllAttributeValueByYachtIdAndCategoryIdAsync(yachtId, categoryId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        /// <summary>
        /// Get Yacht attributes based on category to update
        /// </summary>
        /// <param name="yachtId">ID of Yacht in int format</param>
        /// <param name="categoryId">ID of attribute category in int format</param>
        /// <returns></returns>
        [HttpGet]
        [Route("YachtAttributeValueMgtUpdate/{yachtId}/{categoryId}")]
        public IActionResult GetListUpdateAttributeValueAsync(int yachtId, int categoryId)
        {
            
            var result = _yachtAttributeValueService.GetListUpdateAttributeValueAsync(yachtId, categoryId);
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
            
        }


        
        /// <summary>
        /// Insert attribute for Yacht
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("YachtAttributeValue")]
        public IActionResult InsertYachtAttributeValueAsync([FromBody]YachtAttributeValuesCreateModels model)
        {
            var atrributeIds = model.AttributeFid;
            var result =  _yachtAttributeValueService.CreateAsync(model).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        
        /// <summary>
        /// Update attribute of Yacht with range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("YachtAttributeValues")]
        public IActionResult UpdateYachtAttributeValuesRangeAsync([FromBody]YachtAttributeValuesUpdateRangeModel model)
        {
            var result =  _yachtAttributeValueService.UpdateAttributeValueRangeAsync(model).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();


        }
    }
}