using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using AQConfigurations.Core.Models.PortalLanguages;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IPortalLanguageRequestService
    {
        BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByDomainId(int domainId, string actionUrl = "");
        BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByPortalUID(string portalUniqueId, string actionUrl = "");
        BaseResponse<List<SelectListItem>> GetLanguagesAsSelectListByDomainId(int domainId, string actionUrl = "");
        BaseResponse<List<SelectListItem>> GetLanguagesAsSelectListByDomainUID(string portalUniqueId, string actionUrl = "");
    }
}