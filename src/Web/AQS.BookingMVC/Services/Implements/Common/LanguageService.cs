using AQConfigurations.Core.Models.CommonResources;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Services.Interfaces.Common;
using System.Collections.Generic;

namespace AQS.BookingMVC.Services.Implements.Common
{
    public class LanguageService : ILanguageService
    {

        #region Fields
        private readonly IMultiLanguageService _multiLanguageService;
        public int CurrentLaguageId { get { return _multiLanguageService.CurrentLaguageId; } }
        #endregion

        #region Ctor
        public LanguageService(IMultiLanguageService multiLanguageService)
        {
            _multiLanguageService = multiLanguageService;
        }
        #endregion

        #region Methods
        public List<CommonResourceViewModel> GetCurrentResources()
        {
            return _multiLanguageService.GetCurrentResources();
        }
        public string GetResource(string key)
        {
            return _multiLanguageService.GetResource(key);
        }
        #endregion

    }
}
