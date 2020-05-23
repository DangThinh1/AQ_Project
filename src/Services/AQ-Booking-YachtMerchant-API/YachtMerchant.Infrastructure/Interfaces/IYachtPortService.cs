using APIHelpers.Response;
using System.Collections.Generic;
using YachtMerchant.Core.Models.PortLocation;
using YachtMerchant.Infrastructure.Database.Entities;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtPortService
    {
        BaseResponse<List<PortLocationViewModel>> GetListPortLocation();
        BaseResponse<List<PortLocationViewModel>> GetPortLocationByCityName(string cityName);
        BaseResponse<List<PortLocationViewModel>> GetPortLocationByCountry(string name);
    }
}
