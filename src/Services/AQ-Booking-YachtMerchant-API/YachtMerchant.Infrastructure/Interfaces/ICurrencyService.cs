using YachtMerchant.Core.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface ICurrencyService
    {
        void InitController(ControllerBase controller);
        Task<CurrencyInfoModel> GetCurrencyInfoAsync(string currencyCode);
        CurrencyInfoModel GetCurrencyInfo(string country);
        CurrencyInfoModel GetCurrencyInfo(int merchantId);
        Task<string> GetCultureCodeByCurrencyCode(string currencyCode);    
    }
}
