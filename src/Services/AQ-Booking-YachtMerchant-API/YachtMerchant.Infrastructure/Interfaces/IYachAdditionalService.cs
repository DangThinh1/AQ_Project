using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachAdditionalServices;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachAdditionalService
    {

        BaseResponse<YachtAdditionalServiceDetailViewModel> GetAllAdditionalServiceDetailByAdditionalId(int id);
        BaseResponse<bool> MapServicesToYacht(List<YachtAdditionalServiceControlCreateModel> creatModels);
        BaseResponse<bool> MapDetailsToService(List<YachtAdditionalServiceDetailCreateModel> creatModels);

        BaseResponse<PagedList<YachAdditionalServiceViewModel>> SearchYachtAdditionalService(YachAdditionalServiceSearchModel model);
        BaseResponse<YachAdditionalServiceViewModel> GetAdditionalServiceById(int id);
        BaseResponse<bool> Create(YachAdditionalServiceUpdateModel createModel);
        BaseResponse<bool> Update(YachAdditionalServiceUpdateModel createModel);
        BaseResponse<bool> Delete(int id);
        BaseResponse<bool> IsActivated(int id, bool value);
        BaseResponse<bool> CreateServiceDetail(YachtAdditionalServiceDetailViewModel model);
        BaseResponse<bool> UpdateServiceDetail(YachtAdditionalServiceDetailViewModel model);
        BaseResponse<YachtAdditionalServiceDetailViewModel> GetServiceDetailByServiceId(int serviceId);
        BaseResponse<YachtAdditionalServiceDetailViewModel> GetDetailServiceDetail(int additionalServiceFid, int productId);
        BaseResponse<bool> DeleteServiceDetail(int AdditionalServiceId, int ProductId);
        BaseResponse<bool> CreateServiceControl(YachtAdditionalServiceControlViewModel model);
        BaseResponse<bool> UpdateServiceControl(YachtAdditionalServiceControlViewModel model);
        BaseResponse<YachtAdditionalServiceControlViewModel> GetServiceControlByServiceId(int serviceId);
        BaseResponse<YachtAdditionalServiceControlViewModel> GetDetailServiceControl(int additionalServiceFid, int yachtId);
        BaseResponse<bool> DeleteServiceControl(int AdditionalServiceId, int YachtId);


        BaseResponse<PagedList<YachtAdditionalServiceDetailModel>> AdditionalServiceDetails(int additionalId, YachtAdditionalServiceDetailSearchModel model);
        BaseResponse<bool> CreateAdditionalServiceDetail(YachtAdditionalServiceDetailCreateModel model);

        BaseResponse<PagedList<YachtAdditionalServiceControlModel>> AdditionalServiceControls(int additionalId, YachtAdditionalServiceControlSearchModel model);
        BaseResponse<bool> CreateAdditionalServiceControl(YachtAdditionalServiceControlCreateModel model);
    }
}
