using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtTourCharterDetails;
using YachtMerchant.Core.Models.YachtTourCharters;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCharterService
    {

        /// <summary>
        /// Get infomation of YachtTourCharter By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtTourCharterViewModel> GetInfomationYachtTourCharterById(long id);

        /// <summary>
        /// Create new YachtTourCharter From Source AQ Booking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateCharterFromOriginSource(YachtTourCharterCreateModel model);

        /// <summary>
        /// Create new YachtTourCharter  From Other Source ( out side AQ Booking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateCharterFromOtherSource(CreateTourCharterFromOtherSourceModel model);

        /// <summary>
        /// Update Status Process YachtTourCharter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateStatusAsync (YachtTourCharterConfirmStatusModel model);

        /// <summary>
        /// Delete Yacht Tour Charter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteAsync(long id);

        /// <summary>
        /// Search all Yacht Chartering ,of all yacht with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtTourCharterViewModel>> SearchAllCharterPaging (YachtTourCharterSearchPagingModel model);

        /// <summary>
        /// Search all Yacht Tour Charter ,of [all yacht of Merchant with paging]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtTourCharterViewModel>> SearchAllCharterOfMerchantPaging(YachtTourCharterOfMerchantSearchPagingModel model);

        /// <summary>
        /// Search all Yacht Tour Charter of Tour with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtTourCharterViewModel>> SearchAllCharterOfTourPaging (YachtTourCharterOfTourSearchPagingModel model);

        /// <summary>
        /// Get all Yacht Tour Charter , of all Yacht no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterByTypeNoPaging(YachtTourCharterSearchModel model);


        /// <summary>
        /// Get all Yacht Tour Charter of Merchant by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterOfMerchantByTypeNoPaging(YachtTourCharterOfMerchantSearchModel model);

        /// <summary>
        /// Get all Yacht Tour Charter of Tour by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterViewModel>> GetAllCharterOfTourByTypeNoPaging(YachtTourCharterOfTourSearchModel model);

        /// <summary>
        /// Get Yacht Tour Charter Detail No Paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<YachtTourCharterDetailsModel> GetCharterDetail(long id);


        /// <summary>
        /// Get Yacht Tour Charter Detail with paging --->  is running
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtTourCharterDetailsModel>> GetCharterDetailPaging(YachtTourCharterDetailSearchPagingModel model);

        /// <summary>
        /// Count total charter of merchant by merchant Id with Check EffectiveStartDate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> CalculateTotalReservationOfMerchantByMerchantId(GetTotalReservationOfMerchantModel model);


        /// <summary>
        /// Sum total amount reservation for merchant by ReseravationItemType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<double> CalculateTotalAmountReservationOfMerchantByMerchantId(GetTotalAmountReservationOfMerchantWithItemTypeModel model);


        /// <summary>
        /// Show infomation in dashboard about Reservation Info in date in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardReservationInfo(int yachtId);

        /// <summary>
        ///  Show infomation in dashboard about Request process reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardRequestProcessReservations(int yachtId);

        /// <summary>
        /// Show infomation in dashboard about recent payment reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterDetailModel>> ShowDashboardRecentPaymentReservations(int yachtId);








    }
}
