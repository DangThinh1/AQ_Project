using AccommodationMerchant.Core.Models.HotelFileStreamModel;
using AQBooking.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelFileStreamService
    {
        PagedList<HotelFileStreamViewModel> SearchHotelFileStream(HotelFileStreamSearchModel model);
        HotelFileStreamViewModel GetHotelFileStreamById(int id);
        bool CreateHotelFileStream(HotelFileStreamCreateUpdateModel parameters);
        bool UpdateHotelFileStream(HotelFileStreamCreateUpdateModel parameters);
        bool DeleteHotelFileStream(int id);
    }
}
