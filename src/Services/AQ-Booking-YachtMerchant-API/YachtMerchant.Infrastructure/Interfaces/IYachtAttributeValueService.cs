using AQBooking.Core.Models;
using YachtMerchant.Core.DTO;
using YachtMerchant.Core.Models.YachtAttributeValues;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtAttributeValueService 
    {
        Task<BaseResponse<bool>> CreateAsync(YachtAttributeValuesCreateModels modelCreate);
        Task<BaseResponse<bool>> CreateRangeAsync(List<YachtAttributeValuesCreateModel> modelCreate);

        BaseResponse<List<YachtAttributeValuesViewModel>> GetAllAttributeValueByYachtIdAndCategoryIdAsync(int yachtId, int attributeCategoryId);

        BaseResponse<List<YachtAttributeValuesViewModel>> GetAllAttributeValueByYachtIdAsync(int yachtId);

        Task<BaseResponse<bool>> UpdateAttributeValueRangeAsync(YachtAttributeValuesUpdateRangeModel updateModel);

        BaseResponse<List<YachtAttributeValueMgtUpdateModel>> GetListUpdateAttributeValueAsync(int yachtId, int attributeCategoryId);
    }
}
