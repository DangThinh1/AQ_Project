using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtMerchantInformation;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantInformationService
    {
        BaseResponse<bool> Create(YachtMerchantInformationAddOrUpdateModel createModel);

        BaseResponse<bool> IsActivated(int id, bool value);

        BaseResponse<bool> Delete(int id);

        BaseResponse<PagedList<YachtMerchantInformationViewModel>> Search(YachtMerchantInformationSearchModel searchModel);

        BaseResponse<bool> UpdateDetail(YachtMerchantInformationAddOrUpdateModel updateModel);

        BaseResponse<bool> CreateDetail(YachtMerchantInformationAddOrUpdateModel createModel);

        BaseResponse<List<YachtMerchantInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId);

        BaseResponse<YachtMerchantInformationAddOrUpdateModel> FindInfoDetailById(int id);
        BaseResponse<bool> CheckInforDetail(int id, int inforId);
    }
}