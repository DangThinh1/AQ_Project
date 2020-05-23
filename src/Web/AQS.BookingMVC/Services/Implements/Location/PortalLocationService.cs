using APIHelpers.Response;
using AQConfigurations.Core.Models.PortalLocationControls;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Location;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Location
{
    public class PortalLocationService : ServiceBase, IPortalLocationService
    {
        private readonly ILocationService _locationService;
        private readonly YachtPortalApiUrl _yachtPortalApiUrls;
        public PortalLocationService(ILocationService locationService,
            IOptions<YachtPortalApiUrl> _yachtPortalApiUrlOptions
            )
        {
            _locationService = locationService;
            _yachtPortalApiUrls = _yachtPortalApiUrlOptions.Value;
        }
        public async Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueId(string portalUniqueId)
        {
            try
            {
              
                string url =$"{_baseConfigurationApi}{_yachtPortalApiUrls.PortalLocations.GetLocationsByPortalUniqueId}{portalUniqueId}";
                var response =await _apiExcute.GetData<List<PortalLocationControlViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueIdAndCountryCode(string portalUniqueId, int countryCode)
        {
            try
            {
                string apiUrlAction = string.Format(_yachtPortalApiUrls.PortalLocations.GetLocationsByPortalUniqueIdAndCountryCode, portalUniqueId, countryCode);

                string url =$"{_baseConfigurationApi}{apiUrlAction}";

                var response =await _apiExcute.GetData<List<PortalLocationControlViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<List<PortalLocationControlViewModel>>> GetLocationsByPortalUniqueIdAndCountryName(string portalUniqueId, string countryName)
        {
            try
            {
                   string apiUrlAction= string.Format(_yachtPortalApiUrls.PortalLocations.GetLocationsByPortalUniqueIdAndCountryName, portalUniqueId, countryName);

                string url =$"{_baseConfigurationApi}{apiUrlAction}";

                var response =await _apiExcute.GetData<List<PortalLocationControlViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

       
    }
}
