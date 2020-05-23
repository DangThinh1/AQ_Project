using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class CityRequestService : ConfigurationsRequestServiceBase, ICityRequestService
    {
        private const string GET_ALL = "api/Cities";
        private const string FIND_BY_CITY_NAME = "api/Cities/CityName/";
        private const string GET_BY_COUNTRY_CODE = "api/Cities/CountryCode/";
        private const string GET_BY_COUNTRY_NAME = "api/Cities/CountryName/";
        private const string GET_BY_LIST_CITY_NAMES = "api/Cities/CityNames";
        
        public CityRequestService() : base()
        {
        }

        public BaseResponse<List<CityViewModel>> GetAllCities(string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_ALL;
                string url = _configurationsHost + actionUrl;
                var response = Get<List<CityViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetCitiesByCountryCode(int countryCode, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_BY_COUNTRY_CODE + countryCode.ToString();
                string url = _configurationsHost + actionUrl;
                var response = Get<List<CityViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetCitiesByCountryName(string countryName, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_BY_COUNTRY_NAME + countryName;
                string url = _configurationsHost + actionUrl;
                var response = Get<List<CityViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CityViewModel> FindCityByCityName(string cityName, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = FIND_BY_CITY_NAME + cityName;
                string url = _configurationsHost + actionUrl;
                var response = Get<CityViewModel>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CityViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetCitiesByListCityNames(List<string> cityNames, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_BY_LIST_CITY_NAMES;
                string url = _configurationsHost + actionUrl;
                var response = Post<List<CityViewModel>>(url, cityNames);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}