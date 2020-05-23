using AccommodationMerchant.Core.Models.HotelInventories;
using APIHelpers.Response;
using AQBooking.Core.Helpers;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelInventoryService
    {
        BaseResponse<HotelInventoryViewModel> Find(long id);
        BaseResponse<PagedList<HotelInventoryViewModel>> Search(HotelInventorySearchModel model);
        BaseResponse<bool> Create(HotelInventoryCreateModel model);
        BaseResponse<bool> Update(HotelInventoryUpdateModel model);
        BaseResponse<bool> Activate(long id);
        BaseResponse<bool> Deactivate(long id);
        BaseResponse<bool> Delete(long id);
    }
}