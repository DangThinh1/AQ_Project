using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class ZoneDistrictRequestService : ConfigurationsRequestServiceBase, IZoneDistrictRequestService
    {
        private const string GET_ALL_STATES = "api/ZoneDistricts";
        private const string GET_STATES_BY_CITY_CODE = "api/ZoneDistricts/CityCode/";
        private const string GET_STATES_BY_ZONE_DISTRICT_NAME = "api/ZoneDistricts/ZoneDistrictName/";

        public ZoneDistrictRequestService() : base()
        {
        }

        public BaseResponse<List<StateViewModel>> GetAllStates(string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_ALL_STATES;
                string url = _configurationsHost + actionUrl;
                var response = Get<List<StateViewModel>>(url);
                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg:ex.StackTrace);
            }

        }

        public BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_STATES_BY_CITY_CODE + cityCode.ToString();
                string url = _configurationsHost + actionUrl;
                var response = Get<List<StateViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_STATES_BY_ZONE_DISTRICT_NAME + zoneDistrictName.ToString();
                string url = _configurationsHost + actionUrl;
                var response = Get<List<StateViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}