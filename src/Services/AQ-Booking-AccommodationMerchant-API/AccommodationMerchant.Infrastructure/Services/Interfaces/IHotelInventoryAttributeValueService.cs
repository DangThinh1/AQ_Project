using System.Collections.Generic;
using System.Threading.Tasks;
using APIHelpers.Response;
using AccommodationMerchant.Core.Models.HotelAttributeValues;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelInventoryAttributeValueService
    {
        Task<BaseResponse<bool>> CreateAsync(HotelInventoryAttributeValueCreateModel modelCreate);

        Task<BaseResponse<bool>> CreateRangeAsync(List<HotelInventoryAttributeValueCreateModel> modelCreate);

        BaseResponse<List<HotelInventoryAttributeValueViewModel>> GetAllAttributeValueByInventoryIdAndCategoryId(int inventoryId, int attributeCategoryId);

        BaseResponse<List<HotelInventoryAttributeValueViewModel>> GetAllAttributeValueByInventoryId(int inventoryId);

        Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(HotelInventoryAttributeValueUpdateRangeModel updateModel);

        BaseResponse<List<HotelInventoryAttributeValueMgtUpdateModel>> GetListUpdateAttributeValue(int inventoryId, int attributeCategoryId);
    }
}
