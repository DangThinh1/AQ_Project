using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.EVisaMerchant;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IEVisaMerchantService
    {
        IPagedList<EVisaMerchantViewModel> SearchEVisaMerchant(EVisaMerchantSearchModel model);
        EVisaMerchantViewModel GetEvisaMerchantById(int id);
        bool CreateEvisaMerchant(EVisaMerchantCreateUpdateModel model);
        bool UpdateEvisaMerchant(EVisaMerchantCreateUpdateModel model);
        bool DeleteEvisaMerchant(int id);
        List<SelectListModel> GetEVisaMerchantNoUserSll();
        List<SelectListModel> GetAllEVisaMerchantSll();
    }
}
