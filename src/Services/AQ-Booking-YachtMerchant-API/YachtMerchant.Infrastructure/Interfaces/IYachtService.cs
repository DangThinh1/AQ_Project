using APIHelpers.Response;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Core.Models.YachtPort;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtService
    {
       
        BaseResponse<bool> CreateYacht(YachtCreateModel model);
        BaseResponse<YachtBasicProfileModel> GetYachtBasicProfile(int yachtId);
        BaseResponse<YachtCheckActiveModel> ActiveYacht(int yachtId);
        BaseResponse<bool> SetActiveYacht(int yachtId, bool isActiveOperation );
        BaseResponse<bool> UpdateYachtInfo(YachtUpdateModel model);
        BaseResponse<YachtUpdateModel> GetYachtInfoYacht(int yachtId);
        BaseResponse<bool> UpdateYachtPort(YachtPortViewModel model);
    }
}
