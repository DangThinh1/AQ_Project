using Identity.Core.Conts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using System;
using ExtendedUtility;
using YachtMerchant.Core.Models.YachtTourAttributeValues;
using System.Collections.Generic;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api")]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtTourAttributeValuesController : ControllerBase
    {
        private readonly IYachtTourAttributeValueService _yachtTourAttributeValueService;

        public YachtTourAttributeValuesController(IYachtTourAttributeValueService yachtTourAttributeValueService)
        {
            _yachtTourAttributeValueService = yachtTourAttributeValueService;
        }

        [HttpGet("YachtTourAttributeValues/ByTourId/{tourId}")]
        public IActionResult All(string tourId)
        {
            try
            {
                var id = DecryptStringToInt(tourId);
                var result = _yachtTourAttributeValueService.GetByTourId(id);
                if (result.IsSuccessStatusCode)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet("YachtTourAttributeValues/Update/ByTourId/{tourId}")]
        public IActionResult GetListUpdateAttributeValueAsync(string tourId)
        {
            try
            {
                var id = DecryptStringToInt(tourId);
                var result = _yachtTourAttributeValueService.GetListUpdateAttributeValue(id);
                if (result.IsSuccessStatusCode)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        

        [HttpPost("YachtTourAttributeValues")]
        public IActionResult Create([FromBody] List<YachtTourAttributeValueCreateModel> models)
        {
            var result = _yachtTourAttributeValueService.Create(models);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut("YachtTourAttributeValues")]
        public IActionResult Synchronize([FromBody] List<YachtTourAttributeValueUpdateModel> models)
        {
            var result = _yachtTourAttributeValueService.Synchronize(models);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        //Synchronize

        private int DecryptStringToInt(string encryptedValue)
        {
            try
            {
                var decryptedValue = AQEncrypts.Terminator.Decrypt(encryptedValue);
                var intValue = decryptedValue.ToInt32();
                return intValue;
            }
            catch
            {
                return 0;
            }
        }

    }
}