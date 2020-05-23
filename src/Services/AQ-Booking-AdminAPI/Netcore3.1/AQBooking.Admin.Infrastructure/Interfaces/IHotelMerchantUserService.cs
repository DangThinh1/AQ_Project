using AQBooking.Admin.Core.Models.HotelMerchantUser;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IHotelMerchantUserService
    {
        IPagedList<HotelMerchantUserViewModel> SearchHotelMerchantUser(HotelMerchantUserSearchModel model);
        HotelMerchantUserViewModel GetHotelMerchantUserById(int id);
        bool CreateHotelMerchantUser(HotelMerchantUserCreateUpdateModel model);
        bool UpdateHotelMerchantUser(HotelMerchantUserCreateUpdateModel model);
        bool DeleteHotelMerchantUser(int id);
    }
}
