using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using AQConfigurations.Core.Models.PortalLanguages;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface IPortalLanguageService
    {
        BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByDomainId(int domainId);
        BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByPortalUID(string portalUniqueID);
    }
}
