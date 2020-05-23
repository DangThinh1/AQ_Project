using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.PortalLocationControls;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface IPortalLocationControlService
    {
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueId(string uniqueId);
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryCode(string uniqueId, int countryCode);
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryName(string uniqueId, string countryName);
    }
}