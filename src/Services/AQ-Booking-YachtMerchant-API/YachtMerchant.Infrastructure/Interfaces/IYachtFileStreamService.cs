using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtFileStreams;
using AQBooking.Core.Models;
using AQBooking.Core.Helpers;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtFileStreamService
    {
        BaseResponse<PagedList<YachtFileStreamViewModel>> SearchYachtFileStream(YachtFileStreamSearchModel model);
        BaseResponse<PagedList<YachtFileStreamViewModel>> SearchYachtGallery(YachtFileStreamSearchModel model);
        BaseResponse<YachtFileStreamViewModel> GetYachtFileStreamById(int id);
        Task<BaseResponse<bool>> CreateYachtFileStream(YachtFileStreamCreateModel model);
        Task<BaseResponse<bool>> UpdateYachtFileStream(YachtFileStreamUpdateModel model);
        Task<BaseResponse<bool>> DeleteYachtFileStream(int id);

    }
}
