using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtOtherInformation;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtOtherInformationService
    {
        BaseResponse<PagedList<YacthOtherInformationViewModel>> Search(YachtOtherInformatioSearchModel searchModel);
        BaseResponse<bool> Create(YachtOtherInformationAddOrUpdateModel model);
        BaseResponse<bool> Update(YachtOtherInformationAddOrUpdateModel model);
        BaseResponse<bool> Delete(int id);
        BaseResponse<YacthOtherInformationViewModel> FindById(int id);
        BaseResponse<bool> IsActivated(int id, bool value);
    }
}
