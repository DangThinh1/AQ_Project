using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.PortalLocationControls;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IPortalLocationRequestService
    {
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueId(string portalUniqueId, string apiUrl = "");
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryCode(string portalUniqueId, int countryCode, string apiUrl = "");
        BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryName(string portalUniqueId, string countryName, string apiUrl = "");
    }
}
