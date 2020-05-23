using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonLanguages;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICommonLanguagesServices
    {
        BaseResponse<string> GetLanguageCommonValue(int languageId);
        BaseResponse<List<CommonLanguagesViewModel>> GetAllLanguages();
    }
}
