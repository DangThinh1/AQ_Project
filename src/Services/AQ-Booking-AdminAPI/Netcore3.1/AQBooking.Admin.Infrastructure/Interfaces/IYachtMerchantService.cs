using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchant;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Helpers;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IYachtMerchantService
    {
        IPagedList<YachtMerchantViewModel> SearchYachtMerchant(YachtMerchantSearchModel model);

        bool CreateYachtMerchant(YachtMerchantCreateModel model);

        YachtMerchantViewModel FindYachtMerchantByIdAsync(int id);

        bool UpdateYachtMerchantAsync(YachtMerchantCreateModel model);

        bool DeleteYachtMerchant(int id);

        //For select list
        List<SelectListModel> GetYachtMerchantWithoutAccountSelectList();
        List<SelectListModel> GetYachtMerchantWithoutManagerSelectList();
        List<SelectListModel> GetAllYachtMerchantSelectList();
        List<string> GetListYamHasMerchant();
        List<string> GetListYmHasMerchant();
    }
}