using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.RestaurantMerchant;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IRestaurantMerchantService
    {
        IPagedList<RestaurantMerchantViewModel> SearchRestaurantMerchant(RestaurantMerchantSearchModel model);

        Task<bool> CreateRestaurantMerchant(RestaurantMerchantCreateModel model);

        RestaurantMerchantViewModel FindRestaurantMerchantByIdAsync(int id);

        Task<bool> UpdateRestaurantMerchantAsync(RestaurantMerchantCreateModel model);

        Task<bool> DeleteRestaurantMerchant(int id);

        //For select list
        List<SelectListModel> GetRestaurantMerchantWithoutManagerSelectList();
        List<SelectListModel> GetRestaurantMerchantWithoutAccountSelectList();
        List<SelectListModel> GetAllRestaurantMerchantSelectList();
        List<string> GetListDamHasMerchant();
        List<string> GetListDmHasMerchant();
    }
}