using APIHelpers.Response;

namespace YachtMerchant.Infrastructure.RequestServices.Interfaces
{
    public interface ILocationRequestService
    {
        BaseResponse<string> GetCountryNameById(int id);
        BaseResponse<string> GetCityNameById(int id);
        BaseResponse<string> GetLocationNameById(int id);
    }
}
