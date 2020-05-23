using APIHelpers.Response;
using AccommodationMerchant.Core.Models.Hotels;
using AQBooking.Core.Helpers;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelService
    {
        Task<BaseResponse<HotelBasicProfileModel>> GetHotelBasicProfile(int hotelId);
        BaseResponse<PagedList<HotelViewModel>> Search(HotelSearchModel model);
        BaseResponse<bool> Delete(int id);
        BaseResponse<bool> ActiveForOperation(int id, bool value);
        BaseResponse<HotelViewModel> Find(int id);
        BaseResponse<int> Create(HotelCreateModel model);
        BaseResponse<bool> Setup(HotelCreateModel model);
        BaseResponse<bool> Update(HotelUpdateModel model);
    }
}