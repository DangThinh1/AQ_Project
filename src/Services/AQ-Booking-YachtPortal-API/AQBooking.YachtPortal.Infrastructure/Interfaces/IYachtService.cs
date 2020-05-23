using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.YachtOptions;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtService
    {
        BaseResponse<List<string>> GetListYachtName(YachtSearchModel searchModel);
        YachtPricingPlanDetailsResult PricingPlanDetailsConvertFromJson(string strJson);
        BaseResponse<List<YachtDetailModel>> GetListYacht();
        BaseResponse<List<YachtOptionExclusiveViewModel>> GetAllMerchantYacht(int merchantId);
        BaseResponse<PagedList<YachtViewModel>> GetYachtsByMerchantFId(SearchYachtWithMerchantIdModel searchModel);
        BaseResponse<PagedList<YachtPrivateCharterViewModel>> PrivateCharterSearch(YachtSearchModel searchModel);
        BaseResponse<SaveCharterPaymentResponseViewModel> SaveCharterPrivatePayment(YachtSavePackageServiceModel yachtPackageModel, string PaymentMethod);
        BaseResponse<YachtSingleViewModel> YachtFindingById(string yachtFId);
        BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyExclusive(YachtImageSlideSearchModel searchModel);
        BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyNew(int showAmount);
        BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyPromotion(int showAmount);
        BaseResponse<PagedList<YachtCharterBookingViewModel>> GetBookingList(YachtbookingRequestModel requestModel);
        BaseResponse<YachtCharterBookingViewModel> GetBookingDetail(string charteringId);
        BaseResponse<YachtCharterBookingViewModel> GetBookingByCharteringUniqueId(string uniqueId);

        BaseResponse<List<YachtPackageViewModel>> GetRedisCartStorage(BookingRequestModel requestModel);
        BaseResponse<List<YachtPackageViewModel>> GetRedisCartStorageAll(string hashKey,string key);
        BaseResponse<YachtPackageViewModel> GetRedisCartStorageDetail(string hashKey, string key,string yachtFId);
        BaseResponse<ResponseModel> DeleteRedisCartStorage(RedisCacheYacthRequestModel requestModel);
        BaseResponse<BookingTotalFee> GetRedisCartStorageTotalFee(BookingRequestModel requestModel);

        //----------
        BaseResponse<YachtSingleViewModel> YachtFindingByIdActiveFalse(string yachtFId);


        #region NEW YATCH FUNCTION
        /// <summary>
        /// Search yatch by store procedure
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        BaseResponse<PagedList<YachtSearchItem>> PrivateCharterSearchStoreProcedure(YachtSearchModel searchModel);
        BaseResponse<List<YachtSimilarItem>> YachtSearchSimilarStoreProcedure(YachtSimilarSearchModel searchModel);

        #endregion
    }
}
