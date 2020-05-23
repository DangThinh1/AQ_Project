using System;
using Identity.Core.Conts;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Core.Models.YachtInformation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtInformationController : ControllerBase
    {
        private readonly IYachtInformationService _yachtInformationService;

        public YachtInformationController(IYachtInformationService yachtInformationService)
        {
            _yachtInformationService = yachtInformationService;
        }

        [HttpGet]
        [Route("YachtInformation")]
        public  IActionResult Search([FromQuery] YachtInformationSearchModel model)
        {
            var result = _yachtInformationService.Search(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtInformation/InformationDeatil/{id}")]
        public IActionResult FindDetailsync(int id)
        {
            var result = _yachtInformationService.FindInfoDetailById(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtInformationDetail/LanguageSupport/{infoId}")]
        public IActionResult GetDetail(int infoId)
        {
            var result = _yachtInformationService.GetInfoDetailSupportedByInfoId(infoId);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtInformation")]
        public IActionResult Create([FromBody] YachtInformationCreateModel model)
        {
           
            var result = _yachtInformationService.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
           
        }

        [HttpPost]
        [Route("YachtInformationDetail")]
        public IActionResult CreateDetail([FromBody]YachtInformationCreateModel model)
        {
            var result = _yachtInformationService.CreateDetail(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtInformationDetail")]
        public IActionResult UpdateDetail(YachtInformationCreateModel model)
        {
           
            var result = _yachtInformationService.UpdateDetail(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);

            return BadRequest();
            
        }

        [HttpPut]
        [Route("YachtInformation/IsActivated/{id}/{value}")]
        public IActionResult IsActivated(int id, bool value)
        {
            var result = _yachtInformationService.IsActivated(id, value);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtInformation/{id}")]
        public IActionResult DeleteAsync(int id)
        {
            var result = _yachtInformationService.Delete(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }
    }
}