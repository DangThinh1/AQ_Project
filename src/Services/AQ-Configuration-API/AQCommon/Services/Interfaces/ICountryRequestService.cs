using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Countries;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICountryRequestService
    {
        BaseResponse<List<CountriesViewModel>> GetAllCountries(string actionUrl = "");
        BaseResponse<CountriesViewModel> FindCountryByCountryCode(int countryCode, string actionUrl = "");
    }
}
