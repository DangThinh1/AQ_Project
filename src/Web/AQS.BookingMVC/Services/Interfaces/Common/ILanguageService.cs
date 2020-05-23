using AQConfigurations.Core.Models.CommonResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Common
{
    public interface ILanguageService
    {
        List<CommonResourceViewModel> GetCurrentResources();
        string GetResource(string key);
    }
}
