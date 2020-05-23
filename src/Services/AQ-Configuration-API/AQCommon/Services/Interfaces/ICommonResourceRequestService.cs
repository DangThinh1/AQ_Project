using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonResources;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICommonResourceRequestService
    {
        BaseResponse<List<CommonResourceViewModel>> GetAllResource(int languageID, List<string> type = null, string actionUrl = "");
        BaseResponse<string> GetResourceValue(int lang, string resourceKey, string actionUrl = "");
    }
}
