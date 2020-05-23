

using APIHelpers.Response;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.Countries;
using System.Collections.Generic;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ILocationRequestService
    {
        #region Cities Request Services
        BaseResponse<List<CityViewModel>> GetAllCities(string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByCountryCode(int countryCode, string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByCountryName(string countryName, string actionUrl = "");
        BaseResponse<CityViewModel> FindCityByCityName(string cityName, string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByListCityNames(List<string> cityNames, string actionUrl = "");
        #endregion Cities Request Services

        #region Country Request Services
        BaseResponse<List<CountriesViewModel>> GetAllCountries(string actionUrl = "");
        BaseResponse<CountriesViewModel> FindCountryByCountryCode(int countryCode, string actionUrl = "");
        #endregion Country Request Services

        #region Zone District Request Services
        BaseResponse<List<StateViewModel>> GetAllStates(string actionUrl = "");
        BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode, string actionUrl = "");
        BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName, string actionUrl = "");
        #endregion Zone District Request Services
    }
}
