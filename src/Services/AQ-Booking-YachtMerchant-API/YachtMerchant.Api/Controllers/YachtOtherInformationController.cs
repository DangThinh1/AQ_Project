using AQBooking.Core.Paging;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtOtherInformation;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtOtherInformationController : ControllerBase
    {
        private readonly IYachtOtherInformationService _yachtOtherInformationService;

        public YachtOtherInformationController(IYachtOtherInformationService yachtOtherInformationService)
        {
            _yachtOtherInformationService = yachtOtherInformationService;
        }

        [HttpGet]
        [Route("YachtOtherInformation")]
        public IActionResult Search([FromQuery] YachtOtherInformatioSearchModel model)
        {
            var result = _yachtOtherInformationService.Search(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtOtherInformation/{id}")]
        public IActionResult GetYachtOtherInformation(int id)
        {
            var result = _yachtOtherInformationService.FindById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtOtherInformation")]
        public IActionResult Create([FromBody] YachtOtherInformationAddOrUpdateModel model)
        {
           
            var result = _yachtOtherInformationService.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(model);

            return BadRequest();
            
        }

        [HttpPut]
        [Route("YachtOtherInformation")]
        public IActionResult Update(YachtOtherInformationAddOrUpdateModel model)
        {
            if (model != null)
            {
                var result = _yachtOtherInformationService.Update(model);
                if(result.IsSuccessStatusCode)
                    return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtOtherInformation/IsActivated/{id}/{value}")]
        public IActionResult IsActivated(int id, bool value)
        {
            var result = _yachtOtherInformationService.IsActivated(id, value);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtOtherInformation/{id}")]
        public  IActionResult Delete(int id)
        {
            
            var result = _yachtOtherInformationService.Delete(id);
            if (result.IsSuccessStatusCode)
                return Ok();

            return  BadRequest();
           
        }
    }
}