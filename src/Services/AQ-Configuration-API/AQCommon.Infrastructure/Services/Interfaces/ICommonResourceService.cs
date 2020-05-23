using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonResources;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICommonResourceService
    {
        BaseResponse<List<CommonResourceViewModel>> GetAllResource(int languageID, List<string> type = null);
        BaseResponse<string> GetResourceValue(int lang, string resourceKey);
        BaseResponse<bool> Create(CommonResourceCreateModel model);
        BaseResponse<bool> Update(CommonResourceUpdateModel model);
    }
}
