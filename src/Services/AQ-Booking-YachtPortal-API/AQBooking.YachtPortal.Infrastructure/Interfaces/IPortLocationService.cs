using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.PortLocations;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IPortLocationService
    {
        BaseResponse<List<CityPortLocationViewModel>> GetPortLocationByCity(List<string> cityNames);
        BaseResponse<List<CityPortLocationViewModel>> GetPortLocationByCountry(string countryName);
    }
}
