using System.Collections.Generic;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using APIHelpers.Response;
using AccommodationMerchant.Core.Models.HotelAttributes;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{ 
    public interface IHotelAttributeService 
    {
        Task<BaseResponse<bool>> CreateAsync(HotelAttributeCreateModel model);
        Task<BaseResponse<bool>> UpdateAsync(HotelAttributeUpdateModel model);
        BaseResponse<PagedList<HotelAttributeViewModel>> Search(HotelAttributeSearchModel searchModel);
        Task<BaseResponse<HotelAttributeViewModel>> FindByIdAsync(int id);
        Task<BaseResponse<HotelAttributeViewModel>> FindByNameAsync(string attributeName);
        BaseResponse<List<HotelAttributeViewModel>> SearchByCategoryId(int categoryId);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
