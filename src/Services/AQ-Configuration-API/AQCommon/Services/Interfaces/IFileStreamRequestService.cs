using APIHelpers.Response;
using AQConfigurations.Core.Enums;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IFileStreamRequestService
    {
        BaseResponse<string> GetImageUrlByIdAndRatio(int id, int ratio, string actionUrl = "");
        BaseResponse<string> GetImageUrlByIdAndRatio(int id, ThumbRatioEnum ratio, string actionUrl = "");
    }
}
