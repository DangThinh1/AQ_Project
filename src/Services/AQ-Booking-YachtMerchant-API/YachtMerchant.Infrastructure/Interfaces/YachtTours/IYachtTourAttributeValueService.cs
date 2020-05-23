using APIHelpers.Response;
using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtTourAttributeValues;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourAttributeValueService
    {
        BaseResponse<bool> Create(YachtTourAttributeValueCreateModel model);
        BaseResponse<bool> Create(List<YachtTourAttributeValueCreateModel> models, bool disableSaveChange = false);
        BaseResponse<bool> Synchronize(List<YachtTourAttributeValueUpdateModel> models);
        BaseResponse<List<YachtTourAttributeValueViewModel>> GetByTourId(int id);
        BaseResponse<List<YachtTourAttributeValueMgtModel>> GetListUpdateAttributeValue(int tourId);
    }
}
