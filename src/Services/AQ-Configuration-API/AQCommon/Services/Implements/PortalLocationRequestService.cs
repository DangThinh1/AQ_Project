using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.PortalLocationControls;

namespace AQConfigurations.Core.Services.Implements
{
    public class PortalLocationRequestService : ConfigurationsRequestServiceBase, IPortalLocationRequestService
    {
        private const string GET_LOCATION_BY_UID = "api/PortalLocations/PortalUniqueId/{0}";
        private const string GET_LOCATION_BY_UID_COUNTRY_CODE = "api/PortalLocations/PortalUniqueId/{0}/CountryCode/{1}";
        private const string GET_LOCATION_BY_UID_COUNTRY_NAME = "api/PortalLocations/PortalUniqueId/{0}/CountryName/{1}";

        public PortalLocationRequestService() : base()
        {
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueId(string portalUniqueId, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_LOCATION_BY_UID, portalUniqueId);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<PortalLocationControlViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryCode(string portalUniqueId, int countryCode, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_LOCATION_BY_UID_COUNTRY_CODE, portalUniqueId, countryCode);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<PortalLocationControlViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryName(string portalUniqueId, string countryName, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_LOCATION_BY_UID_COUNTRY_NAME, portalUniqueId, countryName);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<PortalLocationControlViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}