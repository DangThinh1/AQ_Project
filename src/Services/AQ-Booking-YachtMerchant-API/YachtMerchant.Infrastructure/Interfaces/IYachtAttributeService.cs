using AQBooking.Core.Models;
using YachtMerchant.Core.Models.YachtAttribute;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AQBooking.Core.Helpers;
using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtAttributeService 
        
    {
        Task<BaseResponse<bool>> CreateAsync(YachtAttributeCreateModel model);
        Task<BaseResponse<bool>> UpdateAsync(YachtAttributeUpdateModel model);
        BaseResponse<PagedList<YachtAttributeViewModel>> SearchAsync(YachtAttributeSearchModel searchModel);
        Task<BaseResponse<YachtAttributeViewModel>> FindByIdAsync(int id);
        BaseResponse<YachtAttributeViewModel> FindByNameAsync(string attributeName);
        BaseResponse<List<YachtAttributeViewModel>> SearchByCategoryIdAsync(int categoryId);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
