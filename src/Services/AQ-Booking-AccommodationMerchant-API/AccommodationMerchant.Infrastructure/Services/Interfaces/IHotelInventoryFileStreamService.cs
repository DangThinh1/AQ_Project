using AccommodationMerchant.Core.Models.HotelInventoryFileStreamModel;
using AQBooking.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelInventoryFileStreamService
    {
        PagedList<HotelInventoryFileStreamViewModel> SearchHotelInventoryFileStream(HotelInventoryFileStreamSearchModel model);
        HotelInventoryFileStreamViewModel GetHotelInventoryFileStreamById(int id);
        bool CreateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters);
        bool UpdateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters);
        bool DeleteHotelInventoryFileStream(int id);
    }
}
