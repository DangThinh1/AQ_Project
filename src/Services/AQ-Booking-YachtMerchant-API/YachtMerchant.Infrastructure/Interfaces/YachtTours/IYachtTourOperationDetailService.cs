using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourOperationDetails;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourOperationDetailService
    {
        BaseResponse<bool> Create(YachtTourOperationDetailCreateModel model);
        BaseResponse<bool> Delete(int yachtId, int tourId);
        BaseResponse<bool> SetActivated(int yachtId, int tourId, bool value);
        BaseResponse<List<YachtTourOperationDetailViewModel>> FindByTourId(int tourId);
        BaseResponse<List<YachtTourOperationDetailViewModel>> FindByYachtId(int yachtId);
        BaseResponse<PagedList<YachtTourOperationDetailViewModel>> Search(int tourId, YachtTourOperationDetailSearchModel searchModel);
    }
}
