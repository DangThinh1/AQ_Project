using YachtMerchant.Core.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.CommonValues;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface ICommonValueService
    {
        CommonValueViewModel GetCommonValueByGroupInt(string valueGroup, int valueInt);
        CommonValueViewModel GetCommonValueByGroupString(string valueGroup, string valueString);
        CommonValueViewModel GetCommonValueByGroupDouble(string valueGroup, double valueDouble);
        List<CommonValueViewModel> GetListCommonValueByGroup(string valueGroup, SortTypeEnum sortType = SortTypeEnum.Descending);
        List<CommonValueViewModel> GetAllCommonValue();

        CommonValueViewModel GetCommonValueByGroupIntAsync(string valueGroup, int valueInt);
        CommonValueViewModel GetCommonValueByGroupStringAsync(string valueGroup, string valueString);
        CommonValueViewModel GetCommonValueByGroupDoubleAsync(string valueGroup, double valueDouble);
        List<CommonValueViewModel> GetListCommonValueByGroupAsync(string valueGroup, SortTypeEnum sortType = SortTypeEnum.Descending);
        List<CommonValueViewModel> GetAllCommonValueAsync();
    }
}
