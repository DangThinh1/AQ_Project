using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Currencies;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICurrencyService
    {
        BaseResponse<List<CurrencyViewModel>> All();
        BaseResponse<CurrencyViewModel> Find(string currencyCode);
        BaseResponse<CurrencyViewModel> FindByCountryName(string countryName);
    }
}