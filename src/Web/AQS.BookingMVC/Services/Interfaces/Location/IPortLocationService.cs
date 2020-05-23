using APIHelpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Models.PortLocations;

namespace AQS.BookingMVC.Services.Interfaces.Location
{
    public interface IPortLocationService {
        Task<BaseResponse<List<CityPortLocationViewModel>>> PortLocation(List<string> cityName);
        Task<BaseResponse<List<CityPortLocationViewModel>>> GetCityAndZoneDistrictsByPortalUniqueId();
        Task<BaseResponse<List<CityPortLocationViewModel>>> GetCityAndZoneDistrictsByPortalByCountryName(string countryName);
    }
}
