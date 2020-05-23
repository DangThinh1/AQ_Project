using AQBooking.Admin.Core.Models.YachtMerchantAccountMgt;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Core.Helpers;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtMerchantAccMgtService
    {
        IPagedList<YachtMerchantAccMgtViewModel> SearchYachtMerchantAccMgt(YachtMerchantAccMgtSearchModel model);

        Task<bool> CreateYachtMerchantAccMgt(YachtMerchantAccMgtCreateModel model);

        YachtMerchantAccMgtViewModel FindYachtMerchantAccMgtById(int id);

        Task<bool> UpdateYachtMerchantAccMgt(YachtMerchantAccMgtCreateModel model);

        Task<bool> DeleteYachtMerchantAccMgt(int id);

        YachtMerchantAccMgtViewModel GetYachtMerchantAccMgtByMerchantId(int merchantId);
    }
}