using AQBooking.Core.Helpers;
using AQBooking.Core.Paging;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtAttribute;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtAttributeController : ControllerBase
    {
        private readonly IYachtAttributeService _yachtAttributeService;
        public YachtAttributeController(IYachtAttributeService yachtAttributeService)
        {
            _yachtAttributeService = yachtAttributeService;
        }

        [HttpPost]
        [Route("YachtAttribute")]
        public IActionResult CreateAsync([FromBody] YachtAttributeCreateModel model)
        {
            
            var result =  _yachtAttributeService.CreateAsync(model).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(model);
            }

            return NoContent();
            
        }

        [HttpGet]
        [Route("YachtAttributes")]
        public IActionResult SearchAsync([FromQuery]YachtAttributeSearchModel model)
        {
            var result = _yachtAttributeService.SearchAsync(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpGet]
        [Route("YachtAttributes/AttributeCatagory/{id}")]
        public IActionResult SearchByCategoryIdAsync(int id)
        {
            var result = _yachtAttributeService.SearchByCategoryIdAsync(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtAttributes/{id}")]
        public IActionResult FindByIdAsync(int id)
        {
            var result = _yachtAttributeService.FindByIdAsync(id).Result;
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
           
        }

        [HttpGet]
        [Route("YachtAttributes/AttributeName/{name}")]
        public IActionResult FindNameAsync(string name)
        {
            var result = _yachtAttributeService.FindByNameAsync(name);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpPut]
        [Route("YachtAttributes")]
        public IActionResult UpdateAsync([FromBody]YachtAttributeUpdateModel model)
        {
            var result =  _yachtAttributeService.UpdateAsync(model).Result;
            if (result.IsSuccessStatusCode)
                return Ok();
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtAttributes/{id}")]
        public IActionResult DeleteAsync(int id)
        {
            var result =  _yachtAttributeService.DeleteAsync(id).Result;
            if (result.IsSuccessStatusCode)
                return Ok();
            return BadRequest();
        }



    }
}