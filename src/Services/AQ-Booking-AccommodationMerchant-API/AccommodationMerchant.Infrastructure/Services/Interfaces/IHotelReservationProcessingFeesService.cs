using AccommodationMerchant.Core.Models.HotelReservationProcessingFees;
using APIHelpers.Response;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelReservationProcessingFeesService
    {
        /// <summary>
        /// This is method use create new Processing Fees for each Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateReservationProcessingFees(HotelReservationProcessingFeeCreateModel model);

        /// <summary>
        /// This is method create and also Update change status Reservation in Reservation has control by transactions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateReservationProcessingFeesAndChangeStatusReservationTransaction(HotelReservationProcessingFeeWithChangeStatusReservationCreateModel model);

        /// <summary>
        /// This is method use when want edit infomation Reservation Processing Fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateReservationProcessingFees(HotelReservationProcessingFeeUpdateModel model);


        /// <summary>
        /// This is method use get infomation of Reservation Processing Fee By ReservationId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<HotelReservationProcessingFeeViewModel> GetReservationProcessingFeesByReservationId(long id);

    }
}
