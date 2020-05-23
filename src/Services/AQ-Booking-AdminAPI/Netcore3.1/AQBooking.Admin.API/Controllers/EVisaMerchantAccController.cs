using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.EVisaMerchantAccount;
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
    public class EVisaMerchantAccController : ControllerBase
    {
        #region Fields
        private readonly IEVisaMerchantAccService _eVisaMerchantAccService;
        #endregion

        #region Ctor
        public EVisaMerchantAccController(IEVisaMerchantAccService eVisaMerchantAccService)
        {
            this._eVisaMerchantAccService = eVisaMerchantAccService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Search for hotel merchant user matching supplied query
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("EVisaMerchantAccs")]
        public IActionResult SearchEVisaMerchantAcc([FromQuery]EVisaMerchantAccSearchModel model)
        {
            var response = _eVisaMerchantAccService.SearchEVisaMerchantAcc(model);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// Retrieve merchant user by spcified id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("EVisaMerchantAccs/{id}")]
        public IActionResult GetEVisaMerchantAccById(int id)
        {
            var response = _eVisaMerchantAccService.GetEvisaMerchantAccById(id);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        [HttpPost]
        [Route("EVisaMerchantAccs")]
        public IActionResult CreateEVisaMerchant(EVisaMerchantAccCreateUpdateModel parameters)
        {
            var response = _eVisaMerchantAccService.CreateEvisaMerchantAcc(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpPut]
        [Route("EVisaMerchantAccs")]
        public IActionResult UpdateEVisaMerchant(EVisaMerchantAccCreateUpdateModel parameters)
        {
            var response = _eVisaMerchantAccService.UpdateEvisaMerchantAcc(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpDelete]
        [Route("EVisaMerchantAccs")]
        public IActionResult DeleteEVisaMerchantAcc(int id)
        {
            var response = _eVisaMerchantAccService.DeleteEvisaMerchantAcc(id);
            if (response)
                return Ok(response);
            return BadRequest();
        }
        #endregion
    }
}