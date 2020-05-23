using APIHelpers.Response;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.Countries;
using AQConfigurations.Core.Services.Interfaces;
using System.Collections.Generic;

namespace AQConfigurations.Core.Services.Implements
{
    public class LocationRequestService : ConfigurationsRequestServiceBase, ILocationRequestService
    {
        private readonly ICityRequestService _cityService;
        private readonly ICountryRequestService _countryService;
        private readonly IZoneDistrictRequestService _zoneDistrictService;

        public LocationRequestService(
            ICityRequestService cityService, 
            ICountryRequestService countryService,
            IZoneDistrictRequestService zoneDistrictService) : base()
        {
            _cityService = cityService;
            _countryService = countryService;
            _zoneDistrictService = zoneDistrictService;
        }

        #region Cities Request Services
        public BaseResponse<List<CityViewModel>> GetAllCities(string actionUrl = "") 
            => _cityService.GetAllCities(actionUrl);
        public BaseResponse<List<CityViewModel>> GetCitiesByCountryCode(int countryCode, string actionUrl = "") 
            => _cityService.GetCitiesByCountryCode(countryCode, actionUrl);
        public BaseResponse<List<CityViewModel>> GetCitiesByCountryName(string countryName, string actionUrl = "") 
            => _cityService.GetCitiesByCountryName(countryName, actionUrl);
        public BaseResponse<CityViewModel> FindCityByCityName(string cityName, string actionUrl = "")
            => _cityService.FindCityByCityName(cityName, actionUrl);
        public BaseResponse<List<CityViewModel>> GetCitiesByListCityNames(List<string> cityNames, string actionUrl = "")
            => _cityService.GetCitiesByListCityNames(cityNames, actionUrl);
        #endregion Cities Request Services

        #region Country Request Services
        public BaseResponse<List<CountriesViewModel>> GetAllCountries(string actionUrl = "")
            => _countryService.GetAllCountries(actionUrl);
        public BaseResponse<CountriesViewModel> FindCountryByCountryCode(int countryCode, string actionUrl = "")
            => _countryService.FindCountryByCountryCode(countryCode, actionUrl);
        #endregion Country Request Services

        #region Zone District Request Services
        public BaseResponse<List<StateViewModel>> GetAllStates(string actionUrl = "")
            => _zoneDistrictService.GetAllStates(actionUrl);
        public BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode, string actionUrl = "")
            => _zoneDistrictService.GetStatesByCityCode(cityCode, actionUrl);
        public BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName, string actionUrl = "")
            => _zoneDistrictService.GetStatesByZoneDistrictName(zoneDistrictName, actionUrl);
        #endregion Zone District Request Services
    }
}
