using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtTourInformations;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using Identity.Core.Conts;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourInformationController : ControllerBase
    {
        private readonly IYachtTourInformationServices _yachtTourInformationServices;

        public YachtTourInformationController(IYachtTourInformationServices yachtTourInformationServices)
        {
            _yachtTourInformationServices = yachtTourInformationServices;
        }

        [HttpGet]
        [Route("YachtTourInformation")]
        public IActionResult Search([FromQuery] YachtTourInformationSearchModel model)
        {
            var result = _yachtTourInformationServices.Search(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtTourInformation")]
        public IActionResult Create(YachtTourInformationCreateModel model)
        {
            var result = _yachtTourInformationServices.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtTourInformation/IsActivated/{id}/{value}")]
        public IActionResult IsActivated(int id, bool value)
        {
            var result = _yachtTourInformationServices.IsActivated(id, value);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtTourInformation/{id}")]
        public IActionResult DeleteAsync(int id)
        {
            var result = _yachtTourInformationServices.Delete(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtTourInformation/InformationDetail")]
        public IActionResult CreateDetail(YachtTourInformationUpdateDetailModel model)
        {
            var result = _yachtTourInformationServices.CreateDetail(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtTourInformation/InformationDetail")]
        public IActionResult UpdateDetail(YachtTourInformationUpdateDetailModel model)
        {
            var result = _yachtTourInformationServices.UpdateDetail(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtTourInformation/InformationDeatil/{id}")]
        public IActionResult FindDetail(long id)
        {
            var result = _yachtTourInformationServices.FindInfoDetailById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtTourInformation/LanguageSupport/{infoId}")]
        public IActionResult GetLanguageSupport(int infoId)
        {
            var result = _yachtTourInformationServices.GetInfoDetailSupportedByInfoId(infoId);
            if (!result.IsSuccessStatusCode)
                return NotFound();
            return Ok(result);
        }
       
    }
}