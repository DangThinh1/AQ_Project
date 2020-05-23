using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourPricings;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourPricingService
    {
        BaseResponse<bool> Create(int tourId, int yachtId, YachtTourPricingCreateModel model);
        BaseResponse<bool> Update(YachtTourPricingUpdateModel model);
        BaseResponse<bool> Update(List<YachtTourPricingUpdateModel> models);
        BaseResponse<bool> Delete(long id);
        BaseResponse<List<YachtTourPricingDetailModel>> GetDetailByTourId(int tourId);
        BaseResponse<YachtTourPricingViewModel> GetById(long id);
        BaseResponse<List<YachtTourPricingViewModel>> GetByTourIdAndYachtId(int tourId, int yachtId);
        BaseResponse<PagedList<YachtTourPricingViewModel>> Search(int tourId, YachtTourPricingSearchModel model);
        BaseResponse<PagedList<YachtTourPricingDetailModel>> SearchDetail(int tourId, YachtTourPricingSearchModel model);
    }
}
