using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourFileStream;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourFileStreamService
    {
        BaseResponse<bool> Create(YachtTourFileStreamCreateModel model, int tourId);

        BaseResponse<bool> Create(List<YachtTourFileStreamCreateModel> models, int tourId);

        BaseResponse<bool> Update(YachtTourFileStreamUpdateModel model, long fileID);

        BaseResponse<YachtTourFileStreamViewModel> GetFileStreamById(long fileID);

        BaseResponse<List<YachtTourFileStreamViewModel>> GetFileStreamByCategoryId(int yachtTourId, int catId);
        BaseResponse<PagedList<YachtTourFileStreamViewModel>> GetFileStreamsByTourId(int tourId, YachtTourFileStreamSearchModel searchModel);
        BaseResponse<bool> Delete(int id);
    }
}