using APIHelpers.Response;
using AQConfigurations.Core.Models.CommonValues;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.YachtPortal.Web.Interfaces.Common
{
    public interface ICommonValueService
    {
        Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupInt(string valueGroup, int valueInt);
        Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupString(string valueGroup, string valueString);
        Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupDouble(string valueGroup, double valueDouble);
        Task<BaseResponse<List<CommonValueViewModel>>> GetListCommonValueByGroup(string valueGroup);
        Task<BaseResponse<List<CommonValueViewModel>>> GetAllCommonValue();
    }
}
