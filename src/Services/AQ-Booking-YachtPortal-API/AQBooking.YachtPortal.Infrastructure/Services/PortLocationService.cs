using APIHelpers.Response;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AQBooking.YachtPortal.Core.Models.PortLocations;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class PortLocationService : IPortLocationService
    {
        private readonly AQYachtContext _yachtContext;
        public PortLocationService(AQYachtContext yachtSearchContext)
        {
            _yachtContext = yachtSearchContext;
        }

        public BaseResponse<List<CityPortLocationViewModel>> GetPortLocationByPortLocations(List<string> cityNames)
        {
            return null;
        }

        public BaseResponse<List<CityPortLocationViewModel>> GetPortLocationByCity(List<string> cityNames)
        {
            try
            {
                if (cityNames == null)
                    return BaseResponse<List<CityPortLocationViewModel>>.BadRequest();

                List<CityPortLocationViewModel> result = new List<CityPortLocationViewModel>();
                foreach (var city in cityNames)
                {
                    var ports = (_yachtContext.PortLocations.Where(k => k.Deleted == false && k.City == city))
                         .Select(r => new CityPortLocationViewModel
                         {
                             CityName = r.City,
                             CountryName = r.Country,
                             ID = r.Id,
                             UniqueId = r.UniqueId,
                             ZoneDistrict = r.ZoneDistrict,
                             PickupPointName = r.PickupPointName
                         });
                    if (ports != null)
                        result.AddRange(ports.ToList());
                }

                if (result.Count() == 0)
                    return BaseResponse<List<CityPortLocationViewModel>>.NoContent();

                return BaseResponse<List<CityPortLocationViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityPortLocationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityPortLocationViewModel>>GetPortLocationByCountry(string countryName)
        {
            try
            {
                if (string.IsNullOrEmpty(countryName))
                    return BaseResponse<List<CityPortLocationViewModel>>.BadRequest();
                var ports = (_yachtContext.PortLocations.Where(k => k.Deleted == false && k.Country == countryName))
                        .OrderBy(x => x.City)
                        .ThenBy(x => x.ZoneDistrict)
                        .ThenBy(x => x.PickupPointName)
                        .Select(r => new CityPortLocationViewModel
                        {
                            CityName = r.City,
                            CountryName = r.Country,
                            ID = r.Id,
                            UniqueId = r.UniqueId,
                            ZoneDistrict = r.ZoneDistrict,
                            PickupPointName = r.PickupPointName
                        }).ToList();

                if (ports.Count() == 0)
                    return BaseResponse<List<CityPortLocationViewModel>>.NoContent();
                return BaseResponse<List<CityPortLocationViewModel>>.Success(ports);
            }catch(Exception ex)
            {
                return BaseResponse<List<CityPortLocationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
