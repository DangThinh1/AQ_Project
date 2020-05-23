using System.Threading.Tasks;
using AccommodationMerchant.Core.Models.HotelReservationProcessingFees;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HotelReservationProcessingFeesController : ControllerBase
    {
        private readonly IHotelReservationProcessingFeesService _hotelReservationProcessingFeesService;
        public HotelReservationProcessingFeesController(IHotelReservationProcessingFeesService hotelReservationProcessingFeesService)
        {
            _hotelReservationProcessingFeesService = hotelReservationProcessingFeesService;
            
        }


        /// <summary>
        /// Get infomation of Procesing Fee for Reseravtion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservationProcessingFees/{id}")]
        public IActionResult GetHotelReservationProcessingFees(long id)
        {
            var response = _hotelReservationProcessingFeesService.GetReservationProcessingFeesByReservationId(id);
            return Ok(response);
        }


        /// <summary>
        /// Create new Procesing Fee of Reservation 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelReservationProcessingFees")]
        public async Task<IActionResult> CreateReservationProcessingFeesAsync([FromBody]HotelReservationProcessingFeeCreateModel requestModel)
        {
            var response = await _hotelReservationProcessingFeesService.CreateReservationProcessingFees(requestModel);
            return Ok(response);
        }


        /// <summary>
        /// Create new Reservation Procesing Fee and change status reservation
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelReservationProcessingFees/CreateReservationProcessingFeesAndChangeStatusReservation")]
        public async Task<IActionResult> CreateReservationProcessingFeesAndChangeStatusReservationAsync([FromBody]HotelReservationProcessingFeeWithChangeStatusReservationCreateModel requestModel)
        {
            var response = await _hotelReservationProcessingFeesService.CreateReservationProcessingFeesAndChangeStatusReservationTransaction(requestModel);
            return Ok(response);
        }


        /// <summary>
        /// Update Reservation Processing Fee
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("HotelReservationProcessingFees")]
        public async Task<IActionResult> UpdateReservationProcessingFeesAsync([FromBody]HotelReservationProcessingFeeUpdateModel requestModel)
        {
            var response = await _hotelReservationProcessingFeesService.UpdateReservationProcessingFees(requestModel);
            return Ok(response);
        }
    }
}