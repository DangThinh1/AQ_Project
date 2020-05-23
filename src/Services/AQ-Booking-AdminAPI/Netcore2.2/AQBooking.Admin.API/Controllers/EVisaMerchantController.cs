using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.EVisaMerchant;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class EVisaMerchantController : ControllerBase
    {
        #region Fields
        private readonly IEVisaMerchantService _eVisaMerchantService;
        #endregion

        #region Ctor
        public EVisaMerchantController(IEVisaMerchantService eVisaMerchantService)
        {
            this._eVisaMerchantService = eVisaMerchantService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("EVisaMerchants")]
        public IActionResult SearchEVisaMerchant([FromQuery]EVisaMerchantSearchModel model)
        {
            var response = _eVisaMerchantService.SearchEVisaMerchant(model);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        [HttpGet]
        [Route("EVisaMerchants/{id}")]
        public IActionResult GetEVisaMerchantById(int id)
        {
            var response = _eVisaMerchantService.GetEvisaMerchantById(id);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        [HttpPost]
        [Route("EVisaMerchants")]
        public IActionResult CreateEVisaMerchant(EVisaMerchantCreateUpdateModel parameters)
        {
            var response = _eVisaMerchantService.CreateEvisaMerchant(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpPut]
        [Route("EVisaMerchants")]
        public IActionResult UpdateEVisaMerchant(EVisaMerchantCreateUpdateModel parameters)
        {
            var response = _eVisaMerchantService.UpdateEvisaMerchant(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpDelete]
        [Route("EVisaMerchants")]
        public IActionResult DeleteEVisaMerchant(int id)
        {
            var response = _eVisaMerchantService.DeleteEvisaMerchant(id);
            if (response)
                return Ok(response);
            return BadRequest();
        }
        #endregion
    }
}