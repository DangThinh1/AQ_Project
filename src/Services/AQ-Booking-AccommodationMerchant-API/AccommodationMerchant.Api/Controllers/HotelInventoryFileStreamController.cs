using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationMerchant.Core.Models.HotelInventoryFileStreamModel;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/aqres")]
    public class HotelInventoryFileStreamController : ControllerBase
    {
        #region Fields
        private readonly IHotelInventoryFileStreamService _HotelInventoryFileStreamService;
        #endregion

        #region Ctor
        public HotelInventoryFileStreamController(IHotelInventoryFileStreamService HotelInventoryFileStreamService)
        {
            this._HotelInventoryFileStreamService = HotelInventoryFileStreamService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("HotelInventoryFileStreams")]
        public IActionResult SearchHotelInventoryFileStream([FromQuery]HotelInventoryFileStreamSearchModel parameters)
        {
            var response = new PagedList<HotelInventoryFileStreamViewModel>();
            var result = _HotelInventoryFileStreamService.SearchHotelInventoryFileStream(parameters);
            if (result != null && result.Data != null)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("HotelInventoryFileStreams/{id}")]
        public IActionResult GetHotelInventoryFileStreamById(int id)
        {
            var response = new HotelInventoryFileStreamViewModel();
            var result = _HotelInventoryFileStreamService.GetHotelInventoryFileStreamById(id);
            if (result != null)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("HotelInventoryFileStreams")]
        public IActionResult CreateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters)
        {
            var response = false;
            var result = _HotelInventoryFileStreamService.CreateHotelInventoryFileStream(parameters);
            if (result)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("HotelInventoryFileStreams")]
        public IActionResult UpdateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters)
        {
            var response = false;
            var result = _HotelInventoryFileStreamService.UpdateHotelInventoryFileStream(parameters);
            if (result)
            {
                response = result;
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("HotelInventoryFileStreams")]
        public IActionResult DeleteHotelInventoryFileStream(int id)
        {
            var response = false;
            var result = _HotelInventoryFileStreamService.DeleteHotelInventoryFileStream(id);
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