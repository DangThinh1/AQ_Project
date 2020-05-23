using AccommodationMerchant.Core.Models.HotelReservationPaymentLogs;
using APIHelpers.Response;
using System.Collections.Generic;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelReservationPaymentLogsService
    {
        /// <summary>
        /// This is method use get all payment log of  Reservation By ReservationId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationPaymentLogViewModel>> GetReservationPaymentLogsByReservationId(long id);
    }
}
