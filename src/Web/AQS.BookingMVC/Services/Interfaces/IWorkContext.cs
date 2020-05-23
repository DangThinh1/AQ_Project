using AQConfigurations.Core.Models.CommonLanguages;
using Identity.Core.Models.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Interfaces
{
    public interface IWorkContext
    {
        AuthenticateViewModel CurentUser { get; }
        bool IsAuthentication { get; }
        bool IsComingSoon { get; }
        string CurrentLanguageCode { get; }
        List<CommonLanguagesViewModel> ListLanguageCommon { get; }
        int CurrentLanguageId { get; }
    }
}
