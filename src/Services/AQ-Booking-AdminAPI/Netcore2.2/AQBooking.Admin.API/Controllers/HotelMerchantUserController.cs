using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.HotelMerchantUser;
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
    public class HotelMerchantUserController : ControllerBase
    {
        #region Fields
        private readonly IHotelMerchantUserService _hotelMerchantUserService;
        #endregion

        #region Ctor
        public HotelMerchantUserController(IHotelMerchantUserService hotelMerchantUserService)
        {
            this._hotelMerchantUserService = hotelMerchantUserService;
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
        [Route("HotelMerchantUsers")]
        public IActionResult SearchHotelMerchantUser([FromQuery]HotelMerchantUserSearchModel model)
        {
            var response = _hotelMerchantUserService.SearchHotelMerchantUser(model);
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
        [Route("HotelMerchantUsers/{id}")]
        public IActionResult GetHotelMerchantUserById(int id)
        {
            var response = _hotelMerchantUserService.GetHotelMerchantUserById(id);
            if (response != null)
                return Ok(response);
            return BadRequest();
        }

        [HttpPost]
        [Route("HotelMerchantUsers")]
        public IActionResult CreateHotelMerchant(HotelMerchantUserCreateUpdateModel parameters)
        {
            var response = _hotelMerchantUserService.CreateHotelMerchantUser(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpPut]
        [Route("HotelMerchantUsers")]
        public IActionResult UpdateHotelMerchant(HotelMerchantUserCreateUpdateModel parameters)
        {
            var response = _hotelMerchantUserService.UpdateHotelMerchantUser(parameters);
            if (response)
                return Ok(response);
            return BadRequest();
        }

        [HttpDelete]
        [Route("HotelMerchantUsers")]
        public IActionResult DeleteHotelMerchantUser(int id)
        {
            var response = _hotelMerchantUserService.DeleteHotelMerchantUser(id);
            if (response)
                return Ok(response);
            return BadRequest();
        }
        #endregion
    }
}