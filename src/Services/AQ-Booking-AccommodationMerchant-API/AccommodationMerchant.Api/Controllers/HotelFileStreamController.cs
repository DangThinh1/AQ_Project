using AccommodationMerchant.Core.Models.HotelFileStreamModel;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/aqres")]
    public class HotelFileStreamController : ControllerBase
    {
        #region Fields
        private readonly IHotelFileStreamService _hotelFileStreamService;
        #endregion

        #region Ctor
        public HotelFileStreamController(IHotelFileStreamService hotelFileStreamService)
        {
            this._hotelFileStreamService = hotelFileStreamService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("HotelFileStreams")]
        public IActionResult SearchHotelFileStream([FromQuery]HotelFileStreamSearchModel parameters)
        {
            var response = new PagedList<HotelFileStreamViewModel>();
            var result = _hotelFileStreamService.SearchHotelFileStream(parameters);
            if (result != null && result.Data != null)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("HotelFileStreams/{id}")]
        public IActionResult GetHotelFileStreamById(int id)
        {
            var response = new HotelFileStreamViewModel();
            var result = _hotelFileStreamService.GetHotelFileStreamById(id);
            if (result != null)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("HotelFileStreams")]
        public IActionResult CreateHotelFileStream(HotelFileStreamCreateUpdateModel parameters)
        {
            var response = false;
            var result = _hotelFileStreamService.CreateHotelFileStream(parameters);
            if (result)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("HotelFileStreams")]
        public IActionResult UpdateHotelFileStream(HotelFileStreamCreateUpdateModel parameters)
        {
            var response = false;
            var result = _hotelFileStreamService.UpdateHotelFileStream(parameters);
            if (result)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("HotelFileStreams/{id}")]
        public IActionResult DeleteHotelFileStream(int id)
        {
            var response = false;
            var result = _hotelFileStreamService.DeleteHotelFileStream(id);
            if (result)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }
        #endregion
    }
}