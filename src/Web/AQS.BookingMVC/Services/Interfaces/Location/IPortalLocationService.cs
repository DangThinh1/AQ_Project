using APIHelpers.Response;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.PortalLocationControls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Location
{
    public interface IPortalLocationService
    {
        Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueId(string portalUniqueId);
        Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueIdAndCountryCode(string portalUniqueId, int countryCode);
        Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueIdAndCountryName(string portalUniqueId, string countryName);
    }
}
