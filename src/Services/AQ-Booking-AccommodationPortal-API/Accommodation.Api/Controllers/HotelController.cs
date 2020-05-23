using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accommodation.Core.Models.Hotels;
using Accommodation.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accommodation.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        [Route("HotelSearch")]
        public IActionResult Search(HotelSearchModel searchModel)
        {
            var res = _hotelService.Search(searchModel);
            return Ok(res);
        }

        [HttpGet]
        [Route("Hotel/{id}")]
        public IActionResult FindById(int id)
        {
            var res = _hotelService.FindByID(id);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Hotel")]
        public IActionResult Delete([FromQuery]int id)
        {
            var res = _hotelService.Delete(id);
            return Ok(res);
        }
    }
}