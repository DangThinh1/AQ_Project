using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.HotelMerchantMgt;
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
    public class HotelMerchantMgtController : ControllerBase
    {
        #region Fields
        private readonly IHotelMerchantMgtService _hotelMerchantMgtService;
        #endregion

        #region Ctor
        public HotelMerchantMgtController(IHotelMerchantMgtService hotelMerchantMgtService)
        {
            this._hotelMerchantMgtService = hotelMerchantMgtService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Search for hotel merchant manager matching supplied query
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("HotelMerchantMgts")]
        public IActionResult SearchHotelMerchantMgt([FromQuery]HotelMerchantMgtSearchModel model)
        {
            var response = _hotelMerchantMgtService.SearchHotelMerchantMgt(model);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// Retrieve merchant manager by spcified id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("HotelMerchantMgts/{id}")]
        public IActionResult GetHotelMerchantMgtById(int id)
        {
            var response = _hotelMerchantMgtService.GetHotelMerchantMgtById(id);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        [HttpPost]
        [Route("HotelMerchantMgts")]
        public IActionResult CreateHotelMerchant(HotelMerchantMgtCreateUpdateModel parameters)
        {
            var response = _hotelMerchantMgtService.CreateHotelMerchantMgt(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpPut]
        [Route("HotelMerchantMgts")]
        public IActionResult UpdateHotelMerchant(HotelMerchantMgtCreateUpdateModel parameters)
        {
            var response = _hotelMerchantMgtService.UpdateHotelMerchantMgt(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpDelete]
        [Route("HotelMerchantMgts")]
        public IActionResult DeleteHotelMerchantMgt(int id)
        {
            var response = _hotelMerchantMgtService.DeleteHotelMerchantMgt(id);
            if (response)
                return Ok(response);
            return BadRequest();
        }
        #endregion
    }
}