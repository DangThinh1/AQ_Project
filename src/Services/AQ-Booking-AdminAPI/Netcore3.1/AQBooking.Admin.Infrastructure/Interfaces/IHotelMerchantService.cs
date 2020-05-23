using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.HotelMerchant;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IHotelMerchantService
    {
        IPagedList<HotelMerchantViewModel> SearchHotelMerchant(HotelMerchantSearchModel model);
        HotelMerchantViewModel GetHotelMerchantById(int id);
        bool CreateHotelMerchant(HotelMerchantCreateUpdateModel model);
        bool UpdateHotelMerchant(HotelMerchantCreateUpdateModel model);
        bool DeleteHotelMerchant(int id);
        List<SelectListModel> GetHotelMerchantNoUserSll();
        List<SelectListModel> GetAllHotelMerchantSll();
    }
}
