using AQBooking.YachtPortal.Web.Interfaces.Common;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Services.Interfaces.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Areas.Yacht.Controllers
{
    public class YachtLocationController : BaseYachtController
    {

        #region Fields
        private readonly IPortalLocationService _portalLocationService;
        private readonly IPortLocationService _portLocationService;
        private readonly ICommonValueService _commonValueService;
        #endregion

        #region Ctor
        public YachtLocationController(IPortalLocationService portalLocationService,
          IPortLocationService portLocationService, ICommonValueService commonValueService)
        {
            _portalLocationService = portalLocationService;
            _portLocationService = portLocationService;
            _commonValueService = commonValueService;
        }

        #endregion

        #region Methods
        #region Actions
        [HttpGet]
        public async Task<IActionResult> GetListLocation()
        {
            var portLocation = await _portLocationService.GetCityAndZoneDistrictsByPortalUniqueId();
            var responseData = portLocation.GetDataResponse();
            var listCity = new List<SelectListItem>();
            if (responseData.Count > 0)
            {
                listCity = responseData.GroupBy(x =>
                      new { x.CityName, x.CountryName })
                    .Select(x => new SelectListItem
                    {
                        Text = x.Key.CityName,
                        Value = x.Key.CityName.ToLower()
                    }).ToList();

            }
            return Ok(new
            {
                portLocations = responseData,
                listCity
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetListDistrictPostalByCountry(string country)
        {
            var portLocation = await _portLocationService.GetCityAndZoneDistrictsByPortalByCountryName(country);
            var responseData = portLocation.GetDataResponse();
            var listCity = new List<SelectListItem>();
            if (responseData.Count > 0)
            {
                listCity = responseData.GroupBy(x =>
                      new { x.CityName, x.CountryName })
                    .Select(x => new SelectListItem
                    {
                        Text = x.Key.CityName,
                        Value = x.Key.CityName.ToLower()
                    }).ToList();

            }
            return Ok(new
            {
                portLocations = responseData,
                listCity
            });
        }
        #endregion
        #region Utilities

        #endregion
        #endregion

    }
}
