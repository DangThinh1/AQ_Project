using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtLanding;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Infrastructure.Helpers;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtLandingService
    {
        BaseResponse<PagedList<YachtLandingViewModel>> GetYachtByMerchantIDForLanding(SearchYachtWithMerchantIdModel searchModel);
    }
}
