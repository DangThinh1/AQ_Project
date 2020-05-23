using Identity.Core.Conts;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using YachtMerchant.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Core.Models.YachAdditionalServices;
using System;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtAdditionalServicesController : ControllerBase
    {
        #region field && constructor
        private readonly IYachAdditionalService _yachAdditionalService;

        public YachtAdditionalServicesController(IYachAdditionalService yachAdditionalService)
        {
            _yachAdditionalService = yachAdditionalService;
        }
        #endregion

        #region AdditionalServices
        [HttpGet]
        [Route("YachtAdditionalServices")]
        public IActionResult SearchYachtAdditionalSerivce([FromQuery]YachAdditionalServiceSearchModel model)
        {
            var result = _yachAdditionalService.SearchYachtAdditionalService(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtAdditionalServices/{id}")]
        public IActionResult GetServiceById(int id)
        {
            var result = _yachAdditionalService.GetAdditionalServiceById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Route("YachtAdditionalServices")]
        public IActionResult CreateService(YachAdditionalServiceUpdateModel createModel)
        {
            var result = _yachAdditionalService.Create(createModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtAdditionalServices")]
        public IActionResult UpdateService(YachAdditionalServiceUpdateModel createModel)
        {
            var result = _yachAdditionalService.Update(createModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        [HttpDelete]
        [Route("YachtAdditionalServices/{id}")]
        public IActionResult DeleteService(int id)
        {
            var result = _yachAdditionalService.Delete(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        [HttpPut]
        [Route("YachtAdditionalServices/IsActivated/{id}/{value}")]
        public IActionResult IsActivated(int id, bool value)
        {
            var result = _yachAdditionalService.IsActivated(id, value);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

       
        #endregion

        #region ServicesDetail
        [HttpGet]
        [Route("YachtAdditionalServiceDetails/GetAllAdditionalServiceDetailByAdditionalId/{id}")]
        public IActionResult GetAllAdditionalServiceDetailByAdditionalId(int id)
        {
            var result = _yachAdditionalService.GetAllAdditionalServiceDetailByAdditionalId(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtAdditionalServiceDetails/GetServiceDetailByServiceId/{id}")]
        public IActionResult GetServiceDetailByServiceId(int id)
        {
            var result = _yachAdditionalService.GetServiceDetailByServiceId(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("YachtAdditionalServiceDetails/GetDetailServiceDetail/{additionalServiceFid}/{productId}")]
        public IActionResult GetDetailServiceDetail(int additionalServiceFid, int productId)
        {
            var result = _yachAdditionalService.GetDetailServiceDetail(additionalServiceFid, productId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
       

        [HttpPut]
        [Route("YachtAdditionalServiceDetail")]
        public IActionResult UpdateServiceDetail(YachtAdditionalServiceDetailViewModel createModel)
        {
            var result = _yachAdditionalService.UpdateServiceDetail(createModel);
            if (result.IsSuccessStatusCode == false)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete]
        [Route("YachtAdditionalServices/ServiceDetail/{additionalServiceId}/{productId}")]
        public IActionResult DeleteServiceDetail(int additionalServiceId, int productId)
        {
            var result = _yachAdditionalService.DeleteServiceDetail(additionalServiceId, productId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        #endregion

        #region ServicesControl
        [HttpGet]
        [Route("YachtAdditionalServiceControls/GetServiceControlByServiceId/{id}")]
        public IActionResult GetServiceControlByServiceId(int id)
        {
            var result = _yachAdditionalService.GetServiceControlByServiceId(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtAdditionalServiceControls/GetDetailServiceControl/{additionalServiceFid}/{yachtId}")]
        public IActionResult GetDetailServiceControl(int additionalServiceFid, int yachtId)
        {
            var result = _yachAdditionalService.GetDetailServiceControl(additionalServiceFid, yachtId);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
       

        [HttpPut]
        [Route("YachtAdditionalServiceControls")]
        public IActionResult UpdateServiceControl(YachtAdditionalServiceControlViewModel createModel)
        {
            var result = _yachAdditionalService.UpdateServiceControl(createModel);
            if (result.IsSuccessStatusCode == false)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete]
        [Route("YachtAdditionalServices/ServiceControl/{additionalServiceId}/{yachtId}")]
        public IActionResult DeleteServiceControl(int additionalServiceId, int yachtId)
        {
            var result = _yachAdditionalService.DeleteServiceControl(additionalServiceId, yachtId);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        #endregion

        [HttpPost]
        [Route("YachtAdditionalServiceDetails/AdditionalServiceDetailByAdditionalId/{additionalId}")]
        public IActionResult ServiceDetailByProductId(int additionalId, [FromBody]YachtAdditionalServiceDetailSearchModel model)
        {
            var result = _yachAdditionalService.AdditionalServiceDetails(additionalId, model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpPost]
        [Route("YachtAdditionalServiceDetails")]
        public IActionResult CreateServiceDetail(YachtAdditionalServiceDetailCreateModel model)
        {
            
            var result = _yachAdditionalService.CreateAdditionalServiceDetail(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpPost]
        [Route("YachtAdditionalServiceControls/AdditionalServiceControlByAdditionalId/{additionalId}")]
        public IActionResult ServiceControlByProductId(int additionalId, [FromBody]YachtAdditionalServiceControlSearchModel model)
        {
           
            var result = _yachAdditionalService.AdditionalServiceControls(additionalId, model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }

        [HttpPost]
        [Route("YachtAdditionalServiceControls")]
        public IActionResult CreateServiceControl(YachtAdditionalServiceControlCreateModel model)
        {
            var result = _yachAdditionalService.CreateAdditionalServiceControl(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
            
        }
    }
}