using System.Collections.Generic;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using APIHelpers.Response;
using AccommodationMerchant.Core.Models.HotelAttributes;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{ 
    public interface IHotelInventoryAttributeService
    {
        Task<BaseResponse<bool>> CreateAsync(HotelInventoryAttributeCreateModel model);
        Task<BaseResponse<bool>> UpdateAsync(HotelInventoryAttributeUpdateModel model);
        BaseResponse<PagedList<HotelInventoryAttributeViewModel>> Search(HotelInventoryAttributeSearchModel searchModel);
        Task<BaseResponse<HotelInventoryAttributeViewModel>> FindByIdAsync(int id);
        Task<BaseResponse<HotelInventoryAttributeViewModel>> FindByNameAsync(string attributeName);
        BaseResponse<List<HotelInventoryAttributeViewModel>> SearchByCategoryId(int categoryId);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
