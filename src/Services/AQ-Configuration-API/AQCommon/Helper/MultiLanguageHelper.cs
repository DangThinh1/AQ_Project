using AQConfigurations.Core.Services.Interfaces;
using System;

namespace AQConfigurations.Core.Helper
{
    public class MultiLanguageHelper
    {
        private static readonly IMultiLanguageService _multiLanguageService = ConfigurationsInjectionHelper.GetRequiredService<IMultiLanguageService>()
                                                             ?? throw new ArgumentNullException("IMultiLanguageService is null");
        public static int GetLanguage()
        {
            try
            {
                return _multiLanguageService.CurrentLaguageId;
            }
            catch
            {
                return 1;
            }
        }
    }
}