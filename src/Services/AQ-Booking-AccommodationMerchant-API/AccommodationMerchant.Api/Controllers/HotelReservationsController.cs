using System.Threading.Tasks;
using AccommodationMerchant.Core.Models.HotelReservations;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api")]
    public class HotelReservationsController : ControllerBase
    {
        private readonly IHotelReservationsService _hotelReservationsService;
        public HotelReservationsController(IHotelReservationsService hotelReservationsService)
        {
            _hotelReservationsService = hotelReservationsService;
        }


        #region Basic method Reservation

        /// <summary>
        /// Get infomation of reservation by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/GetInfomationReservation/{id}")]
        public IActionResult GetInfomationReservationById(long id)
        {
            var response = _hotelReservationsService.GetInfomationReservationById(id);
            return Ok(response);
        }

        /// <summary>
        /// Create new Reservation from Origin source AQBooking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelReservations/CreateReservationFromOriginSource")]
        public async Task<IActionResult> CreateReservationFromOriginSource([FromBody]HotelReservationCreateModel model)
        {
            var response = await _hotelReservationsService.CreateReservationFromOriginSource(model);
            return Ok(response);
        }

        /// <summary>
        /// Create new reservation from Other source (outsite aqbooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HotelReservations/CreateReservationFromOtherSource")]
        public async Task<IActionResult> CreateYachtCharteringFromOtherSourceAsync([FromBody]HotelCreateReservationFromOtherSourceModel model)
        {
            var response = await _hotelReservationsService.CreateReservationFromOtherSource(model);
            return Ok(response);
        }

        /// <summary>
        /// Change status of Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("HotelReservations/ConfirmStatus")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody]HotelReservationConfirmStatusModel model)
        {
            var response = await _hotelReservationsService.UpdateStatusAsync(model);
            return Ok(response);
        }

        /// <summary>
        /// Delete reservation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("HotelReservations/{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var response = await _hotelReservationsService.DeleteAsync(id);
            return Ok(response);
        }

        #endregion


        #region  Search reservation 

        /// <summary>
        /// Search all reservation of Merchant by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/SearchAllReservationOfMerchant")]
        public IActionResult SearchAllReservationsForMerchant([FromQuery]ReservationForMerchantSearchPagingModel model)
        {
            var response = _hotelReservationsService.SearchAllReservationOfMerchantPaging(model);
            return Ok(response);
        }


        /// <summary>
        /// Search all reservation of Hotel by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/SearchAllReservationOfHotel")]
        public IActionResult SearchAllReservationsForHotel([FromQuery]ReservationOfHotelSearchPagingModel model)
        {
            var response = _hotelReservationsService.SearchAllReservationOfHotelPaging(model);
            return Ok(response);
        }



        /// <summary>
        /// Search all reservation by type with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/SearchAllReservation")]
        public IActionResult SearchAllReservationsPaging([FromQuery]ReservationSearchPagingModel model)
        {
            var response = _hotelReservationsService.SearchAllReservationPaging(model);
            return Ok(response);
        }

        #endregion



        #region Get Reservation Detail  

       
        /// <summary>
        /// Get detail reservation with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/Detail")]
        public IActionResult GetReservationDetail([FromQuery]ReservationDetailSearchPagingModel model)
        {
            var response = _hotelReservationsService.GetReservationDetail(model);
            return Ok(response);
        }

        #endregion



        #region Get Reservation 

        /// <summary>
        /// Get all  reservation by type no paging
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/GetAllReservationByTypeNoPaging/{type}")]
        public IActionResult GetAllReservationByTypeNoPaging([FromQuery]long type)
        {
            var response = _hotelReservationsService.GetAllReservationByTypeNoPaging(type);
            return Ok(response);
        }


        /// <summary>
        /// Get all  reservation by type paging
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/GetAllReservationByTypePaging/{type}")]
        public IActionResult GetAllReservationByTypePaging([FromQuery]long type)
        {
            var response = _hotelReservationsService.GetAllReservationByTypePaging(type);
            return Ok(response);
        }


        /// <summary>
        /// Get all reservation of Merchant by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/GetAllReservationOfMerchantByTypeNoPaging")]
        public IActionResult GetAllReservationOfMerchantByTypeNoPaging([FromQuery]ReservationOfMerchantSearchModel model)
        {
            var response = _hotelReservationsService.GetAllReservationOfMerchantByTypeNoPaging(model);
            return Ok(response);
        }


        /// <summary>
        /// Get all  reservation of Hotel by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/GetAllReservationOfHotelByTypeNoPaging")]
        public IActionResult GetAllReservationOfHotelByTypeNoPaging([FromQuery]ReservationOfHotelSearchModel model)
        {
            var response = _hotelReservationsService.GetAllReservationOfHotelByTypeNoPaging(model);
            return Ok(response);
        }
        #endregion


        #region Show infomation reservation to home dashboard

        /// <summary>
        /// Show infomation in dashboard about reservation infomation in date in hotel
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/ShowDashboardReservationInfo/{hotelId}")]
        public IActionResult ShowDashboardReservationInfo([FromQuery]int hotelId)
        {
            var response = _hotelReservationsService.ShowDashboardReservationInfo(hotelId);
            return Ok(response);
        }

        /// <summary>
        /// Show infomation in dashboard about request process reservation in hotel
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/ShowDashboardRequestProcessReservations/{hotelId}")]
        public IActionResult ShowDashboardRequestProcessReservations([FromQuery]int hotelId)
        {
            var response = _hotelReservationsService.ShowDashboardRequestProcessReservations(hotelId);
            return Ok(response);
        }

        /// <summary>
        /// Show infomation in dashboard about recent payment reservation in hotel
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HotelReservations/ShowDashboardRecentPaymentReservations/{hotelId}")]
        public IActionResult ShowDashboardRecentPaymentReservations([FromQuery]int hotelId)
        {
            var response = _hotelReservationsService.ShowDashboardRecentPaymentReservations(hotelId);
            return Ok(response);

        }

        #endregion


    }
}