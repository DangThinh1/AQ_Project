using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtCharteringDetails;
using YachtMerchant.Core.Models.YachtCharterings;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtCharteringService
    {

        /// <summary>
        /// Get infomation of Yacht Chartering By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtCharteringsViewModel> GetInfomationYachtCharteringById(long id);

        /// <summary>
        /// Create new Yacht Chartering  From Source AQbooking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateCharteringFromOriginSource(YachtCharteringsCreateModel model);

        /// <summary>
        /// Create new Yacht Chartering  From Other Source ( outside AQBooking)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateCharteringFromOtherSource(CreateCharteringFromOtherSourceModel model);

        /// <summary>
        /// Update Status Process Yacht Chartering
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateStatusAsync (YachtCharteringsConfirmStatusModel model);

        /// <summary>
        /// Delete Chartering
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteAsync(long id);

        /// <summary>
        /// Search all Yacht Chartering ,of all yacht with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringPaging (YachtCharteringsSearchPagingModel model);

        /// <summary>
        /// Search all Yacht Chartering ,of [all yacht of Merchant with paging]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringOfMerchantPaging(YachtCharteringsOfMerchantSearchPagingModel model);

        /// <summary>
        /// Search all Yacht Chartering of Yacht with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtCharteringsViewModel>> SearchAllYachtCharteringOfYachtPaging (YachtCharteringsOfYachtSearchPagingModel model);

        /// <summary>
        /// Get all Yacht Chartering , of all Yacht no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringByTypeNoPaging(YachtCharteringsSearchModel model);


        /// <summary>
        /// Get all Yacht Chartering of Merchant by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringOfMerchantByTypeNoPaging(YachtCharteringsOfMerchantSearchModel model);

        /// <summary>
        /// Get all Yacht Chartering of Yacht by type no paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringsViewModel>> GetAllYachtCharteringOfYachtByTypeNoPaging(YachtCharteringsOfYachtSearchModel model);

        /// <summary>
        /// Get Yacht Chartering detail No Paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<YachtCharteringDetailsModel> GetYachtCharteringDetail(long id);

        /// <summary>
        /// Get Yacht Chartering detail with paging --> Version 1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtCharteringsDetailModel>> GetYachtCharteringDetail(YachtCharteringsDetailSearchPagingModel model);

        /// <summary>
        /// Get Yacht Chartering details with paging --->  Version 2 ==> is running
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtCharteringDetailsModel>> GetYachtCharteringDetails(YachtCharteringsDetailSearchPagingModel model);

        /// <summary>
        /// Count total chatering of merchant by merchant Id with Check EffectiveStartDate
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
        BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardReservationInfo(int yachtId);

        /// <summary>
        ///  Show infomation in dashboard about Request process reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardRequestProcessReservations(int yachtId);

        /// <summary>
        /// Show infomation in dashboard about recent payment reservation in Yacht
        /// </summary>
        /// <param name="yachtId"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringsDetailModel>> ShowDashboardRecentPaymentReservations(int yachtId);








    }
}
