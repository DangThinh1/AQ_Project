using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace AQS.BookingAdmin.Services.Interfaces.Common
{
  public interface ICommonLanguageService
  {
    BaseResponse<List<SelectListItem>> GetListLanguage(string portalUniqueId = "", string apiUrl = "");
  }
}
