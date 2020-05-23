using System.Collections.Generic;
using System.Threading.Tasks;
using APIHelpers.Response;
using AccommodationMerchant.Core.Models.HotelAttributeValues;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelAttributeValueService 
    {
        Task<BaseResponse<bool>> CreateAsync(HotelAttributeValueCreateModel modelCreate);

        Task<BaseResponse<bool>> CreateRangeAsync(List<HotelAttributeValueCreateModels> modelCreate);

        BaseResponse<List<HotelAttributeValueViewModel>> GetAllAttributeValueByHotelIdAndCategoryId(int hotelId, int attributeCategoryId);

        BaseResponse<List<HotelAttributeValueViewModel>> GetAllAttributeValueByHotelId(int hotelId);

        Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(HotelAttributeValueUpdateRangeModel updateModel);

        BaseResponse<List<HotelAttributeValueMgtUpdateModel>> GetListUpdateAttributeValue(int hotelId, int attributeCategoryId);
    }
}
