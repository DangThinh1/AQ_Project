using APIHelpers.Response;
using AQBooking.Admin.Core.Models.CommonLanguague;
using AQConfigurations.Core.Models.PortalLanguages;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Services.Interfaces.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Common
{
  public class CommonLanguageService : BaseService, ICommonLanguageService
  {
    #region Field
    private readonly string _bookingPortalUniqueId = "K140Q2XAJAAF";
    #endregion
    #region Ctor
    public CommonLanguageService()
    {
     
     
    }
    #endregion
    #region Method    
    public BaseResponse<List<SelectListItem>> GetListLanguage(string portalUniqueId = "", string apiUrl = "")
    {
      try
      {
        apiUrl = $"{_baseConfigurationAPIUrl}{_adminPortalApiUrl.ConfigurationAPI.AllLanguages}";
        var response = _aPIExcute.GetData<List<CommonLanguaguesViewModel>>(apiUrl, null).Result;
        if (response.IsSuccessStatusCode)
        {
          var list = response.ResponseData.Select(k => new SelectListItem(k.LanguageName, k.Id.ToString())).ToList();
          return BaseResponse<List<SelectListItem>>.Success(list);
        }
        return BaseResponse<List<SelectListItem>>.BadRequest();
      }
      catch (Exception ex)
      {
        return BaseResponse<List<SelectListItem>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
      }
    }
    #endregion
  }
}
