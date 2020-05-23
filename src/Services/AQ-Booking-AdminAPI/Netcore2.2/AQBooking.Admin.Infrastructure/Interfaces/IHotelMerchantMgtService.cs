using AQBooking.Admin.Core.Models.HotelMerchantMgt;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IHotelMerchantMgtService
    {
        IPagedList<HotelMerchantMgtViewModel> SearchHotelMerchantMgt(HotelMerchantMgtSearchModel model);
        HotelMerchantMgtViewModel GetHotelMerchantMgtById(int id);
        bool CreateHotelMerchantMgt(HotelMerchantMgtCreateUpdateModel model);
        bool UpdateHotelMerchantMgt(HotelMerchantMgtCreateUpdateModel model);
        bool DeleteHotelMerchantMgt(int id);
    }
}
