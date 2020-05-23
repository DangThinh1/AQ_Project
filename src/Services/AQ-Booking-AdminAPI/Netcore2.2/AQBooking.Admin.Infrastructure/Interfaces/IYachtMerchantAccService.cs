using AQBooking.Admin.Core.Models.YachtMerchantAccount;
using System.Threading.Tasks;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Core.Paging;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtMerchantAccService
    {
        IPagedList<YachtMerchantAccViewModel> SearchYachtMerchantAcc(YachtMerchantAccSearchModel model);

        bool CreateYachtMerchantAcc(YachtMerchantAccCreateModel model);

        YachtMerchantAccViewModel FindYachtMerchantAccById(int id);

        bool UpdateYachtMerchantAcc(YachtMerchantAccCreateModel model);

        bool DeleteYachtMerchantAcc(int id);

        YachtMerchantAccViewModel GetYachtMerchantAccByMerchatnId(int merchantId);
    }
}