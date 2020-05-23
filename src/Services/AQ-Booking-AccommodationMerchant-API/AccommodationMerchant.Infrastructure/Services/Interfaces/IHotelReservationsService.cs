using AccommodationMerchant.Core.Models.HotelReservationDetails;
using AccommodationMerchant.Core.Models.HotelReservations;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelReservationsService
    {

        /// <summary>
        /// Get infomation of Reservation By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<HotelReservationViewModel> GetInfomationReservationById(long id);

        /// <summary>
        /// Create new reservation  From Source AQbooking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateReservationFromOriginSource(HotelReservationCreateModel model);


        /// <summary>
        /// Create new reservation From Other Source ( outside AQBooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateReservationFromOtherSource(HotelCreateReservationFromOtherSourceModel model);

        /// <summary>
        /// Update Status Process Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateStatusAsync(HotelReservationConfirmStatusModel model);

        /// <summary>
        /// Delete Reservation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteAsync(long id);


        /// <summary>
        /// Search all reservation with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationPaging(ReservationSearchPagingModel model);

        /// <summary>
        /// Search all reservation ,of all Hotel with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationOfMerchantPaging(ReservationForMerchantSearchPagingModel model);


        /// <summary>
        /// Search all reservation of Hotel with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<HotelReservationViewModel>> SearchAllReservationOfHotelPaging(ReservationOfHotelSearchPagingModel model);
        
        
        /// <summary>
        /// Get Reservation With paging
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        BaseResponse<PagedList<HotelReservationViewModel>> GetAllReservationByTypePaging(long Type = 1);


        /// <summary>
        /// Get Reservation With paging
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationViewModel>> GetAllReservationByTypeNoPaging(long Type = 1);


        /// <summary>
        /// Get all reservation, of all Hotel no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationViewModel>> GetAllReservationOfMerchantByTypeNoPaging(ReservationOfMerchantSearchModel model);

        /// <summary>
        /// Get all Reservation of Hotel by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationViewModel>> GetAllReservationOfHotelByTypeNoPaging(ReservationOfHotelSearchModel model);

       

        /// <summary>
        /// Show Reservation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<HotelReservationDetailModel>> GetReservationDetail(ReservationDetailSearchPagingModel model);

        /// <summary>
        /// Show infomation in dashboard about Reservation Info in date in Hotel
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardReservationInfo(int hotelId);

        /// <summary>
        ///  Show infomation in dashboard about Request process reservation in Hotel
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardRequestProcessReservations(int hotelId);

        /// <summary>
        /// Show infomation in dashboard about recent payment reservation in Hotel
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<HotelReservationShowDashBoardModel>> ShowDashboardRecentPaymentReservations(int hotelId);

    }


}
