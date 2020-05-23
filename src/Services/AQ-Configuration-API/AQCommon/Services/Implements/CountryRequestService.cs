using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Countries;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class CountryRequestService : ConfigurationsRequestServiceBase, ICountryRequestService
    {
        private const string GET_ALL_COUNTRIES = "api/Countries";
        private const string FIND_COUNTRY_BY_COUNTRYCODE = "api/Countries/CountryCode/";
        public CountryRequestService() : base()
        {
        }

        public BaseResponse<List<CountriesViewModel>> GetAllCountries(string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = GET_ALL_COUNTRIES;
                string url = _configurationsHost + actionUrl;
                var response = Get<List<CountriesViewModel>>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CountriesViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CountriesViewModel> FindCountryByCountryCode(int countryCode, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(actionUrl))
                    actionUrl = FIND_COUNTRY_BY_COUNTRYCODE;
                string url = _configurationsHost + actionUrl + countryCode;
                var response = Get<CountriesViewModel>(url);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CountriesViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}