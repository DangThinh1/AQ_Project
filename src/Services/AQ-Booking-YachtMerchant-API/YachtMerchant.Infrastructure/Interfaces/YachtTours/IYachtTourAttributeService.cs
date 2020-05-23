using APIHelpers.Response;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourAttributes;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourAttributeService
    {
        BaseResponse<List<YachtTourAttributeViewModel>> All();
        BaseResponse<bool> Create(YachtTourAttributeCreateModel model);
        BaseResponse<bool> Update(YachtTourAttributeUpdateModel model, int id);
        BaseResponse<bool> Delete(int id);
    }
}