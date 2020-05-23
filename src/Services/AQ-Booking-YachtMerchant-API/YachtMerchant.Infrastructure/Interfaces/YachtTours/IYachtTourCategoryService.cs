using APIHelpers.Response;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourCategory;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCategoryService
    {
        BaseResponse<List<YachtTourCategoryViewModel>> GetAll();
    }
}
