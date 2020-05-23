using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.HotelMerchant;
using AQBooking.Admin.Core.Paging;
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
    public class HotelMerchantController : ControllerBase
    {
        #region Fields
        private readonly IHotelMerchantService _hotelMerchantService;
        #endregion

        #region Ctor
        public HotelMerchantController(IHotelMerchantService hotelMerchantService)
        {
            this._hotelMerchantService = hotelMerchantService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Search for hotel merchant matching supplied query
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("HotelMerchants")]
        [ProducesResponseType(typeof(IPagedList<HotelMerchantViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult SearchHotelMerchant([FromQuery]HotelMerchantSearchModel model)
        {
            var response = _hotelMerchantService.SearchHotelMerchant(model);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// Retrieve merchant by spcified id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("HotelMerchants/{id}")]
        public IActionResult GetHotelMerchantById(int id)
        {
            var response = _hotelMerchantService.GetHotelMerchantById(id);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelMerchants")]
        public IActionResult CreateHotelMerchant(HotelMerchantCreateUpdateModel parameters)
        {
            var response = _hotelMerchantService.CreateHotelMerchant(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("HotelMerchants")]
        public IActionResult UpdateHotelMerchant(HotelMerchantCreateUpdateModel parameters)
        {
            var response = _hotelMerchantService.UpdateHotelMerchant(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("HotelMerchants")]
        public IActionResult DeleteHotelMerchant(int id)
        {
            var response = _hotelMerchantService.DeleteHotelMerchant(id);
            if (response)
                return Ok(response);
            return BadRequest();
        }
        #endregion
    }
}