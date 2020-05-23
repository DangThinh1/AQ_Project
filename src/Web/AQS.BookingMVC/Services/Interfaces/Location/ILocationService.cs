using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Location
{
   public interface ILocationService
    {
        Task<List<CountriesViewModel>> GetCountries();
        Task<CountriesViewModel> GetCountryByCode(int countryCode);
        Task<List<CityViewModel>> GetCities();
        Task<List<CityViewModel>> GetCitiesByCountryCode(int countryCode);
        Task<List<CityViewModel>> GetCitiesByCountryName(string countryName);
        Task<List<StateViewModel>> GetZoneDistrictsByCountryName(string countryName);
        Task<List<StateViewModel>> GetZoneDistrictsByCityCode(int cityCode);
        Task<List<CityViewModel>> GetCityDistrictLstByCityLst(List<string> citiesLst = null);
    }
}
