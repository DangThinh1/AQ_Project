using APIHelpers.Response;
using AQConfigurations.Core.Models.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICountriesService
    {
        //The same GetAllCountryAsync() but not use Async and return BaseResponse format
        BaseResponse<List<CountriesViewModel>> GetAllCountries();
        //The same GetCountryByCodeAsync(int countryCode) but not use Async and return BaseResponse format
        BaseResponse<CountriesViewModel> FindByCountryCode(int countryCode);

        Task<List<CountriesViewModel>> GetAllCountryAsync();
        Task<CountriesViewModel> GetCountryByCodeAsync(int countryCode);
    }
}
