using APIHelpers.Response;
using YachtMerchant.Infrastructure.Helpers;
using YachtMerchant.Infrastructure.RequestServices.Interfaces;

namespace YachtMerchant.Infrastructure.RequestServices.Implements
{
    public class LocationRequestService : AQRequestServiceBase, ILocationRequestService
    {
        private string _apiServer;
        public LocationRequestService() : base()
        {
            _apiServer = ApiUrlHelper.ConfigurationApi;
        }

        public BaseResponse<string> GetCountryNameById(int id)
        {
            return BaseResponse<string>.Success("Country");
        }

        public BaseResponse<string> GetCityNameById(int id)
        {
            return BaseResponse<string>.Success("City");
        }

        public BaseResponse<string> GetLocationNameById(int id)
        {
            return BaseResponse<string>.Success("Location");
        }
    }
}
