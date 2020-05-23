using AQConfigurations.Core.Models.CommonResources;
using System.Collections.Generic;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IMultiLanguageService
    {
        void Clear();
        string GetResource(string key);
        int CurrentLaguageId { get; }
        List<CommonResourceViewModel> GetCurrentResources();
    }
}
