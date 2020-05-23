using AQBooking.Admin.Core.Models.RestaurantMerchantAccountMgt;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IRestaurantMerchantAccMgtService
    {
        IPagedList<RestaurantMerchantAccMgtViewModel> SearchRestaurantMerchantAccMgt(RestaurantMerchantAccMgtSearchModel model);

        Task<bool> CreateRestaurantMerchantAccMgt(RestaurantMerchantAccMgtCreateModel model);

        RestaurantMerchantAccMgtViewModel FindRestaurantMerchantAccMgtById(int id);

        Task<bool> UpdateRestaurantMerchantAccMgt(RestaurantMerchantAccMgtCreateModel model);

        Task<bool> DeleteRestaurantMerchantAccMgt(int id);

        RestaurantMerchantAccMgtViewModel GetRestaurantMerchantAccMgtByMerchantId(int merchantId);
    }
}