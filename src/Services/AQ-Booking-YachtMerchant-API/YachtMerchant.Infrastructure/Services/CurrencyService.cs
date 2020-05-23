using YachtMerchant.Core.Models.Currency;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Linq;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Database;

namespace YachtMerchant.Infrastructure.Services
{
    public class CurrencyService:ServiceBase, ICurrencyService
    {
        public CurrencyService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<string> GetCultureCodeByCurrencyCode(string currencyCode)
        {
            var item = await _context.Currencies.FindAsync(currencyCode);
            if (item != null)
                return item.CultureCode;
            else
                return string.Empty;
        }

        public async Task<CurrencyInfoModel> GetCurrencyInfoAsync(string currencyCode)
        {
            var item = await _context.Currencies.FindAsync(currencyCode);
            if(item!=null)
            {
                CurrencyInfoModel result = new CurrencyInfoModel();
                result.InjectFrom(item);
                return result;
            }
            return new CurrencyInfoModel();
        }

        public CurrencyInfoModel GetCurrencyInfo(string country)
        {
            if(_context.Currencies.Any(k=>k.Country.ToLower()==country))
            {
                var item =  _context.Currencies.First(k=>k.Country.ToLower()==country);                                  
                CurrencyInfoModel result = new CurrencyInfoModel();
                result.InjectFrom(item);
                return result;
            }
            return new CurrencyInfoModel();
        }

        public CurrencyInfoModel GetCurrencyInfo(int merchantId)
        {
            var country = _context.YachtMerchants.FirstOrDefault(x=>x.Id==merchantId);
            if(country!= null)
            {
               string countryName = country.Country;
                if (_context.Currencies.Any(k => k.Country.ToLower() == countryName.Trim()))
                {
                    var item = _context.Currencies.First(k => k.Country.ToLower() == countryName);
                    CurrencyInfoModel result = new CurrencyInfoModel();
                    result.InjectFrom(item);
                    return result;
                }
            }
          
            return new CurrencyInfoModel();
        }
    }
}
