using Identity.Core.Conts;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Core.Models.YachtMerchantInformation;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtMerchantInformationController : ControllerBase
    {
        private readonly IYachtMerchantInformationService _yachtMerchantInformationService;
        public YachtMerchantInformationController(IYachtMerchantInformationService yachtMerchantInformationService)
        {
            _yachtMerchantInformationService = yachtMerchantInformationService;
        }

        [HttpGet]
        [Route("YachtMerchantInformation")]
        public IActionResult Search([FromQuery] YachtMerchantInformationSearchModel model)
        {
            var result = _yachtMerchantInformationService.Search(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantInformation/InformationDeatil/{id}")]
        public IActionResult FindDetail(int id)
        {
            var result = _yachtMerchantInformationService.FindInfoDetailById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantInformation/LanguageSupport/{infoId}")]
        public IActionResult GetLanguageSupport(int infoId)
        {
            var result = _yachtMerchantInformationService.GetInfoDetailSupportedByInfoId(infoId);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantInformation")]
        public IActionResult Create([FromBody] YachtMerchantInformationAddOrUpdateModel model)
        {
            var result = _yachtMerchantInformationService.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantInformation/InformationDetail")]
        public IActionResult CreateDetail([FromBody]YachtMerchantInformationAddOrUpdateModel model)
        {
                var result = _yachtMerchantInformationService.CreateDetail(model);
                if (result.IsSuccessStatusCode)
                    return Ok(result);
                return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantInformation/InformationDetail")]
        public IActionResult UpdateDetail(YachtMerchantInformationAddOrUpdateModel model)
        {
                var result = _yachtMerchantInformationService.UpdateDetail(model);
                if (result.IsSuccessStatusCode)
                    return Ok(result);
                return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantInformation/IsActivated/{id}/{value}")]
        public IActionResult IsActivated(int id, bool value)
        {
            var result = _yachtMerchantInformationService.IsActivated(id, value);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtMerchantInformation/{id}")]
        public IActionResult DeleteAsync(int id)
        {
                var result = _yachtMerchantInformationService.Delete(id);
                if (result.IsSuccessStatusCode)
                    return Ok(result);
                return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantInformation/CheckInformationDetail/{id}/{inforId}")]
        public IActionResult CheckInformationDetail(int id, int inforId)
        {
            var result = _yachtMerchantInformationService.CheckInforDetail(id, inforId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}