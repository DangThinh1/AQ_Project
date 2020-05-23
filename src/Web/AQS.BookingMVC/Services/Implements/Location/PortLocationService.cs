using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.PortLocations;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Location;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Location
{
  
    public class PortLocationService : ServiceBase, IPortLocationService
    {
        #region Field
        private readonly IPortalLocationService _portalLocationService;
        private readonly YachtPortalApiUrl _yachtPortalApiUrls;
        private string _baseYatchApiUrl = ApiUrlHelper.YachtPortalApi;
        #endregion

        #region Ctor
        public PortLocationService(
            IPortalLocationService portalLocationService,
            IOptions<YachtPortalApiUrl> yachtPortalApiUrlOptions
            )
        {
            _portalLocationService = portalLocationService;
            _yachtPortalApiUrls = yachtPortalApiUrlOptions.Value;


        }
        #endregion

        #region Methods
        
        public async Task<BaseResponse<List<CityPortLocationViewModel>>> PortLocation(List<string> cityNames)
        {
            try
            {
                string paramater = ConvertToUrlParameter(cityNames, "cityNames");
                string url = $"{_baseYatchApiUrl}{_yachtPortalApiUrls.PortLocation.PortLocationByCity}{paramater}";              
                var response =await _apiExcute.GetData<List<CityPortLocationViewModel>>(url, null);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityPortLocationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<List<CityPortLocationViewModel>>> GetCityAndZoneDistrictsByPortalUniqueId()
        {
            try
            {
                var citiyReponse =await _portalLocationService.GetLocationsByPortalUniqueId(PortalLocationConstant.YachtUniqueId);
                if (citiyReponse != null && citiyReponse.IsSuccessStatusCode && citiyReponse.ResponseData != null)
                {
                    List<string> citiesLst = citiyReponse.ResponseData.Select(k => k.CityName).ToList();
                    List<CityPortLocationViewModel> resultAPI =(await PortLocation(citiesLst)).ResponseData;
                    return BaseResponse<List<CityPortLocationViewModel>>.Success(resultAPI);
                }
                return BaseResponse<List<CityPortLocationViewModel>>.NotFound(new List<CityPortLocationViewModel>());
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityPortLocationViewModel>>.InternalServerError(new List<CityPortLocationViewModel>(), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<BaseResponse<List<CityPortLocationViewModel>>> GetCityAndZoneDistrictsByPortalByCountryName(string countryName)
        {
            try
            {

                string url = $"{_baseYatchApiUrl}{_yachtPortalApiUrls.PortLocation.PortLocationByCountry}/{countryName}";
                var response = await _apiExcute.GetData<List<CityPortLocationViewModel>>(url, null);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityPortLocationViewModel>>.InternalServerError(new List<CityPortLocationViewModel>(), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

    }
}
