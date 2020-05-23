using APIHelpers.Response;
using System.Threading.Tasks;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICitiesService
    {
        BaseResponse<CityViewModel> FindByCityName(string cityName);
        BaseResponse<List<CityViewModel>> GetAllCities();
        BaseResponse<List<CityViewModel>> GetByCountryCode(int countryCode);
        BaseResponse<List<CityViewModel>> GetByCountryName(string countryName);
        BaseResponse<List<CityViewModel>> GetByListCityNames(List<string> cities);

        #region .
        Task<List<CityViewModel>> GetAllCityAsync();
        Task<List<CityViewModel>> GetCityByCodeAsync(int countryCode);
        Task<CityViewModel> GetCityByNameAsync(string name);
        Task<List<CityViewModel>> GetCityByCountryNameAsync(string countryName);
        Task<List<CityViewModel>> GetCityDistrictLstByCityLstAsync(List<string> cityLst = null);
        #endregion
    }
}