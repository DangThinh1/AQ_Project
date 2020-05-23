using AQBooking.Admin.Core.Models.RestaurantMerchantAccount;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IRestaurantMerchantAccService
    {
        IPagedList<RestaurantMerchantAccViewModel> SearchRestaurantMerchantAcc(RestaurantMerchantAccSearchModel model);

        Task<bool> CreateRestaurantMerchantAcc(RestaurantMerchantAccCreateModel model);

        RestaurantMerchantAccViewModel FindRestaurantMerchantAccById(int id);

        Task<bool> UpdateRestaurantMerchantAcc(RestaurantMerchantAccCreateModel model);

        Task<bool> DeleteRestaurantMerchantAcc(int id);

        RestaurantMerchantAccViewModel GetRestaurantMerchantAccByMerchatnId(int merchantId);
    }
}