using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonValues;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface ICommonValueRequestService
    {
        BaseResponse<CommonValueViewModel> Find(int id, int lang = 1, string actionUrl = "");
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupInt(string valueGroup, int valueInt, int lang = 1, string apiUrl = "");
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupString(string valueGroup, string valueString, int lang = 1, string apiUrl = "");
        BaseResponse<CommonValueViewModel> GetCommonValueByGroupDouble(string valueGroup, double valueDouble, int lang = 1, string apiUrl = "");
        BaseResponse<List<CommonValueViewModel>> GetListCommonValueByGroup(string valueGroup, int lang = 1, string apiUrl = "");
        BaseResponse<List<CommonValueViewModel>> GetAllCommonValue(int lang = 1, string apiUrl = "");
    }
}
