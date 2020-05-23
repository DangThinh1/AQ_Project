using APIHelpers.Request;
using AQBooking.YachtPortal.Web.Interfaces;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.Countries;
using AQS.BookingMVC.Services.Interfaces.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Location
{
    public class LocationService : ServiceBase, ILocationService
    {
        private string basicAuthToken = string.Empty;
        private readonly BaseRequest<object> requestParams;

        public LocationService()
        {
            basicAuthToken = _apiExcute.GetBasicAuthToken("AESAPI", "Sysadmin@2019$$");
            requestParams = new BaseRequest<object>();
        }
        public async Task<List<CountriesViewModel>> GetCountries()
        {
            string url = _baseConfigurationApi + "api/Country/GetCountry";
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<CountriesViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }

        public async Task<CountriesViewModel> GetCountryByCode(int countryCode)
        {
            string url = _baseConfigurationApi + "api/Country/GetCountryByCode?code=" + countryCode;
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<CountriesViewModel>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }
        public async Task<List<CityViewModel>> GetCities()
        {
            string url = _baseConfigurationApi + "api/City/GetCity";
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<CityViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }

        public async Task<List<CityViewModel>> GetCitiesByCountryCode(int countryCode)
        {
            string url = _baseConfigurationApi + "api/City/GetCityByCode?code=" + countryCode;
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<CityViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }

        public async Task<List<CityViewModel>> GetCitiesByCountryName(string countryName)
        {
            string url = _baseConfigurationApi + "api/City/GetCityByName?name=" + countryName;
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<CityViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }

        public async Task<List<StateViewModel>> GetZoneDistrictsByCountryName(string countryName)
        {
            string url = _baseConfigurationApi + "api/State/GetZonedistrictByCityName?countryName=" + countryName;
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<StateViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }


        public async Task<List<StateViewModel>> GetZoneDistrictsByCityCode(int cityCode)
        {
            string url = _baseConfigurationApi + "api/State/GetZonedistrictByCityCode?cityCode=" + cityCode;
            var response = await _apiExcute.GetData<object>(url, null, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<StateViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }

        public async Task<List<CityViewModel>> GetCityDistrictLstByCityLst(List<string> citiesLst = null)
        {
            string url = _baseConfigurationApi + "api/City/GetCityDistrictLstByCityLst";
            requestParams.RequestData = citiesLst;
            var response = await _apiExcute.PostData<object, object>(url, APIHelpers.HttpMethodEnum.POST, requestParams, basicAuthToken);
            var ViewModel = JsonConvert.DeserializeObject<List<CityViewModel>>(JsonConvert.SerializeObject(response.ResponseData));
            if (response.IsSuccessStatusCode)
                return ViewModel;
            else
                return null;
        }
    }
}
