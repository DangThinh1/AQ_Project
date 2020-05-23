using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Areas.Yacht.Models;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtDetailViewModel = AQS.BookingMVC.Areas.Yacht.Models.YachtDetailViewModel;

namespace AQS.BookingMVC.Areas.Yacht.Controllers
{
    public class YachtDetailController : BaseYachtController
    {
        #region Service
        private readonly IYatchService _yatchService;
        private readonly IMultiLanguageService _multiLanguageService;
        #endregion

        #region Contructor
        public YachtDetailController(IYatchService yatchService,
            IMultiLanguageService multiLanguageService)
        {
            _yatchService = yatchService;
            _multiLanguageService = multiLanguageService;
        }
        #endregion

        #region Controller Method
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get yatch detail
        /// </summary>
        /// <param name="id">Encrypt Id</param>
        /// <returns>Yatch Detail</returns>
        [Route("yacht/{id}", Name = "YachtDetail")]
        public async Task<IActionResult> Detail(string id, YachtPassParamsModel passDataModel)
        {
            var model = new YachtDetailViewModel();
            model.YachtPassParamsModel = passDataModel;
            model.YachtPassParamsModel.YatchId = id;
            //#region Get Data Pass from Search
            //if (!string.IsNullOrEmpty(q))
            //{
            //    model.YachtPassParamsModel = YachtPassParamsModel.GetModelFromEncryptString(q);
            //}
            //#endregion

            #region Get Yacht
            var result = await _yatchService.YachtFindingById(id);
            if (result.ResponseData == null) return NotFound();
            #endregion

            #region Get Overview
            var responseOverview = await _yatchService.GetYatchOverview(id, _multiLanguageService.CurrentLaguageId);
            var overview = responseOverview.GetDataResponse();
            model.FullDescriptions = overview.FullDescriptions;
            model.ShortDescriptions = overview.ShortDescriptions;
            #endregion

            #region Get Detail
            var lstAttribute = new List<string>()
            {
                "Bathroom","Bedding"
            };
            var responseDetail = await _yatchService.GetYatchDetail(id, 1, true, lstAttribute);
            model.Details = responseDetail.GetDataResponse();
            #endregion

            #region Get Yatch Amenities
            var responseAmenities = await _yatchService.GetYatchAmenities(id, 2, false, lstAttribute);
            model.Amenities = responseAmenities.GetDataResponse();
            #endregion

            #region Get image of Yatch
            // Exterior pictures
            var responseExteriorPicture = await _yatchService.GetYachtFileStream(id, (int)FileCategoriesEnum.ExtoriorPicture);
            var listExteriorPicture = responseExteriorPicture.GetDataResponse();

            // Interior pictures
            var responseInteriorPicture = await _yatchService.GetYachtFileStream(id, (int)FileCategoriesEnum.InteriorPicture);
            var listInteriorPicture = responseInteriorPicture.GetDataResponse();

            // Other Pictures
            var responseOtherPicture = await _yatchService.GetYachtFileStream(id, (int)FileCategoriesEnum.OtherPicture);
            var listOtherPicture = responseOtherPicture.GetDataResponse();

            // All picture
            var allPicture = new List<YachtFileStreamViewModel>();
            allPicture.AddRange(listExteriorPicture);
            allPicture.AddRange(listInteriorPicture);
            allPicture.AddRange(listOtherPicture);

            model.ListYatchImage = allPicture;
            #endregion

            return View(model);
        }
        #endregion
    }
}