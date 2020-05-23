using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using AQBooking.YachtPortal.Web.Interfaces.Common;
using AQConfigurations.Core.Models.CommonValues;
using AQEncrypts;
using AQS.BookingMVC.Areas.Yacht.Models;
using AQS.BookingMVC.Infrastructure.AQPagination;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Services.Implements.Common;
using AQS.BookingMVC.Services.Interfaces;
using AQS.BookingMVC.Services.Interfaces.Common;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AQS.BookingMVC.Areas.Yacht.Controllers
{
    public class YachtSearchController : BaseYachtController
    {
        #region Fields
        private readonly IYatchService _yatchService;
        private readonly ICommonValueService _commonValueService;
        private readonly IFileStreamService _fileStreamService;
        private readonly IWebHelper _webHelper;
        private readonly ILanguageService _languageService;
        #endregion

        #region Ctor
        public YachtSearchController(IYatchService yatchService,
            ICommonValueService commonValueService,
            IFileStreamService fileStreamService,
            IWebHelper webHelper,
            ILanguageService languageService
            )
        {
            _yatchService = yatchService;
            _commonValueService = commonValueService;
            _fileStreamService = fileStreamService;
            _webHelper = webHelper;
            _languageService = languageService;
        }
        #endregion

        #region Methods
        #region Actions
        public IActionResult Index()
        {
            return RedirectToAction("YachtSearchIndex");
        }
        [Route("yacht/search")]
        public async Task<IActionResult> YachtSearchIndex(YachtSearchModel searchModel)
        {
            if (searchModel.Passengers == 0)
                searchModel.Passengers = 1;

            var model = await PreparingYatchSearchViewModel(searchModel);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> YachtSearchResult(YachtSearchModel searchModel)
        {            
            var model = await PreparingYarchItemsModel(searchModel);
            string url = ($"{_webHelper.GetHostName()}{Url.Action("YachtSearchIndex").TrimStart('/')}{_webHelper.GetQueryString()}").ToLower();
            return Ok(new
            {
                model,
                url
            });

        }
        [HttpGet]
        public async Task<IActionResult> YachtSearchSimilarResult(string yachtId,string country, YachtSimilarSearchModel searchModel)
        {
            searchModel.ExcludeYachtID = Convert.ToInt32(Terminator.Decrypt(yachtId));
            var yatchApiResponse = await _yatchService.SearchSimilar(searchModel);
            var responseData = yatchApiResponse.GetDataResponse();
            var searchModelPass = new YachtSearchModel
            {
                CheckIn = searchModel.CheckIn,
                CheckOut = searchModel.CheckOut,
                City = searchModel.City,
                Passengers = searchModel.NumOfPassenger,
                Country=country
            };
            foreach (var item in responseData)
            {
                await PreparingYachtCustomProperties(item);

                PreparingYachtLink(item, searchModelPass);

            }
            return Ok(responseData);

        }
        #endregion

        #region Utilities
        private async Task<YachtSearchViewModel> PreparingYatchSearchViewModel(YachtSearchModel searchModel)
        {
            var model = new YachtSearchViewModel();
            model.YachtSearchModel = searchModel;

            var typeResponse = await _commonValueService.GetListCommonValueByGroup(CommonValueConstant.YACHT_TYPE);
            model.YachtTypes = typeResponse.GetDataResponse();
            return model;

        }
        private async Task<PagedListClient<YachtSearchItem>> PreparingYarchItemsModel(YachtSearchModel model)
        {
            if (string.IsNullOrEmpty(model.CheckIn))
                model.CheckIn = DateTime.Now.ToString("dd-MMM-yyyy");
            if (string.IsNullOrEmpty(model.CheckOut))
                model.CheckOut = DateTime.Now.AddDays(7).ToString("dd-MMM-yyyy");
            model.PageSize = model.PageSize > 0 ? model.PageSize : 15;
            var yatchApiResponse = await _yatchService.Search(model);
            var responseData = yatchApiResponse.GetDataResponse();
            foreach (var item in responseData.Data)
            {
                await PreparingYachtCustomProperties(item);
                PreparingYachtLink(item, model);

            }
            return responseData;

        }
        private async Task PreparingYachtCustomProperties(YachtItem item)
        {
            if (item.FileStreamFid > 0)
            {
                item.CustomProperties["FileThumbUrl"] = await _fileStreamService.GetFileById(item.FileStreamFid.Value, AQBooking.YachtPortal.Core.Enum.ThumbRatioEnum.half);
            }
            else
            {
                item.CustomProperties["FileThumbUrl"] = Url.Content(CommonValueConstant.NO_IMAGE_PATH);
            }
            item.CustomProperties["PricingType"] = item.PricingTypeResKey != null ? _languageService.GetResource(item.PricingTypeResKey) ?? "" : "";
            if (item.FromPrice != null)
            {
                item.CustomProperties["PriceFromText"] = item.FromPrice?.FormatCurrency(item.CultureCode);
            }

        }
        private void PreparingYachtLink(YachtItem item, YachtSearchModel searchModel)
        {
            string yachtId = Terminator.Encrypt(item.YachtID);
         
            var passParams = new YachtPassParamsModel(0, 0, searchModel);
            //string paramsData = passParams.EncryptData();
            string detailLink = Url.RouteUrl("YachtDetail", new { id = yachtId });
            detailLink+= CommonHelper.ConvertToUrlParameter(passParams).ToLower();
            item.CustomProperties["DetailLink"] = detailLink;
        }




        #endregion
        #endregion

    }
}