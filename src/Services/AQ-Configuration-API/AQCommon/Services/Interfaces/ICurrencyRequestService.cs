using APIHelpers.Response;
using AQConfigurations.Core.Models.Currencies;
using System.Collections.Generic;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICurrencyRequestService
    {
        BaseResponse<List<CurrencyViewModel>> All(string actionUrl = "");
        BaseResponse<CurrencyViewModel> Find(string currencyCode, string actionUrl = "");
        BaseResponse<CurrencyViewModel> FindByCountryName(string countryName, string actionUrl = "");
    }
}