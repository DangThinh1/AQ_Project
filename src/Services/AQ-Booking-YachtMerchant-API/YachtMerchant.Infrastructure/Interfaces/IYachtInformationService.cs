using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtInformation;
using YachtMerchant.Core.Models.YachtInformationDetail;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtInformationService
    {
        BaseResponse<bool> Create(YachtInformationCreateModel createModel);
        BaseResponse<bool> IsActivated(int id, bool value);
        BaseResponse<bool> Delete(int id);
        BaseResponse<YachtInformationViewModel> FindById(int id);
        BaseResponse<PagedList<YachtInformationViewModel>> Search(YachtInformationSearchModel searchModel);
        BaseResponse<bool> UpdateDetail(YachtInformationCreateModel updateModel);
        BaseResponse<bool> CreateDetail(YachtInformationCreateModel createModel);
        BaseResponse<List<YachtInformationDetailSupportModel>> GetInfoDetailSupportedByInfoId(int infoId);
        BaseResponse<YachtInformationCreateModel> FindInfoDetailById(int id);
    }
}
