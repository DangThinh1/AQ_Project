using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICityRequestService
    {
        BaseResponse<List<CityViewModel>> GetAllCities(string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByCountryCode(int countryCode, string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByCountryName(string countryName, string actionUrl = "");
        BaseResponse<CityViewModel> FindCityByCityName(string cityName, string actionUrl = "");
        BaseResponse<List<CityViewModel>> GetCitiesByListCityNames(List<string> cityNames, string actionUrl = "");
    }
}