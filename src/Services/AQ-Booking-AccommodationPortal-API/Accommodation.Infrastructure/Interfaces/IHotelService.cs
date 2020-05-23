using Accommodation.Core.Models.Hotels;
using APIHelpers.Response;
using AQBooking.Core.Helpers;

namespace Accommodation.Infrastructure.Interfaces
{
    public interface IHotelService
    {
        BaseResponse<PagedList<HotelViewModel>> Search(HotelSearchModel searchModel);
        BaseResponse<bool> Delete(int id);
        BaseResponse<HotelViewModel> FindByID(int id);
    }
}
