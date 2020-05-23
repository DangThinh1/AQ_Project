using APIHelpers.Response;
using YachtMerchant.Core.Models;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface ICheckHealthService
    {
        BaseResponse<CheckHealthModel> ServerInfo();
        BaseResponse<bool> IsGoodHealth();
    }
}
