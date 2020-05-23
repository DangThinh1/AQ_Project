using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelReservationPaymentLogsController : ControllerBase
    {
        private readonly IHotelReservationPaymentLogsService _hotelReservationPaymentLogsService;
        public HotelReservationPaymentLogsController(IHotelReservationPaymentLogsService hotelReservationPaymentLogsService)
        {
            _hotelReservationPaymentLogsService = hotelReservationPaymentLogsService;
        }


        /// <summary>
        /// Get log payment of reservation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservationPaymentLogs/{id}")]
        public IActionResult GetReservationPaymentLogs(long id)
        {
            var response = _hotelReservationPaymentLogsService.GetReservationPaymentLogsByReservationId(id);
            return Ok(response);
        }
    }
}