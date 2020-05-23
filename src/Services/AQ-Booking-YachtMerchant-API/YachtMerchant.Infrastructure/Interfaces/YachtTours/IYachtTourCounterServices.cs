using APIHelpers.Response;
using YachtMerchant.Core.Models.YachtTourCounters;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCounterServices
    {
        BaseResponse<YachtTourCounterViewModel> FindById(int id);
        BaseResponse<bool> Create(YachtTourCounterCreateModel createModel);
        BaseResponse<bool> Update(YachtTourCounterViewModel createModel);
    }
}
