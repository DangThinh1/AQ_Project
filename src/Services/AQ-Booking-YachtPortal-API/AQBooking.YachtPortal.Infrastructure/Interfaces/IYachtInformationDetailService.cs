using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtInformationDetailService
    {
        BaseResponse<YachtInformationDetailViewModel> GetInfomationDetailByYachtFId(string yachtFId, int lang);
    }
}
