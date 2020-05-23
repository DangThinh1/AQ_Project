using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourInformations;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourInformationServices
    {
        BaseResponse<bool> Create(YachtTourInformationCreateModel model);

        BaseResponse<bool> IsActivated(int id, bool value);

        BaseResponse<bool> Delete(int id);

        BaseResponse<PagedList<YachtTourInformationViewModel>> Search(YachtTourInformationSearchModel model);

        BaseResponse<bool> UpdateDetail(YachtTourInformationUpdateDetailModel model);

        BaseResponse<bool> CreateDetail(YachtTourInformationUpdateDetailModel model);

        BaseResponse<YachtTourInformationUpdateDetailModel> FindInfoDetailById(long id);

        BaseResponse<List<YachtTourInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId);
    }
}