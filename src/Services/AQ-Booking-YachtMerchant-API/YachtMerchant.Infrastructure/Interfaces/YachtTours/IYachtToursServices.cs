using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTours;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtToursServices
    {
        BaseResponse<string> Create(YachtTourCreateModel model);
        BaseResponse<List<YachTourViewModel>> GetToursByMerchantFid(int merchantId);
        BaseResponse<List<YachTourDetailModel>> GetTourDetailsByMerchantFid(int merchantId);
        BaseResponse<PagedList<YachTourDetailModel>> Search(int merchantId, YachtTourSearchModel model);
        BaseResponse<YachTourViewModel> GetTourById(int yachTourId);
        BaseResponse<List<YachtTourSelectListModel>> GetYachtTourSelectItem();
        BaseResponse<bool> Update(YachtTourUpdateModel model, int tourId);
        BaseResponse<bool> Delete(int tourId);
        BaseResponse<bool> Activate(int tourId);
        BaseResponse<bool> Deactivate(int tourId);

        BaseResponse<YachtTourCheckActiveModel> ActiveTour(int tourId);
    }
}
