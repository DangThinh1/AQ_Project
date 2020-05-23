using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourNonOperationDays;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourNonOperationDayService
    {
        BaseResponse<bool> Create(List<YachtTourNonOperationDayCreateModel> models);
        BaseResponse<YachtTourNonOperationDayViewModel> FindById(int id);
        BaseResponse<PagedList<YachtTourNonOperationDayViewModel>> FindByTourFid(int tourId, YachtTourNonOperationDaySearchModel searchModel);
        BaseResponse<List<YachtTourNonOperationDayViewModel>> FindByYachtFid(int yachtId);
        BaseResponse<bool> Delete(int id);
    }
}
