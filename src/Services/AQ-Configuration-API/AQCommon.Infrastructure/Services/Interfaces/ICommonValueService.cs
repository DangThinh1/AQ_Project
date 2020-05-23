using AQConfigurations.Core.Enums;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonValues;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface ICommonValueService
    {
        BaseResponse<bool> Create(CommonValueCreateModel model);
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupInt(string valueGroup, int valueInt, int lang = 1);
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupString(string valueGroup, string valueString, int lang = 1);
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupDouble(string valueGroup, double valueDouble, int lang = 1);
        BaseResponse<List<CommonValueViewModel>> GetListCommonValueByGroup(string valueGroup, int lang = 1, SortTypeEnum sortType = SortTypeEnum.Descending);
        BaseResponse<List<CommonValueViewModel>> GetAllCommonValue(int lang = 1);
        BaseResponse<CommonValueViewModel> Find(int id, int lang = 1);
    }
}
