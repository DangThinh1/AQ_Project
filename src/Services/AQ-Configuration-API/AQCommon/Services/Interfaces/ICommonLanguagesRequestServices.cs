using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonLanguages;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICommonLanguagesRequestServices
    {
        BaseResponse<List<CommonLanguagesViewModel>> GetAllLanguages(string actionUrl = "");
        BaseResponse<List<CommonLanguagesViewModel>> GetAllCommonValue(string actionUrl = "");
    }
}
