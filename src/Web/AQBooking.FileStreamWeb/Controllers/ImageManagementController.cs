using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStreamWeb.Helpers.AQPagination;
using AQBooking.FileStreamWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AQBooking.FileStreamWeb.Controllers
{
    public class ImageManagementController : Controller
    {
        #region Fields
        private readonly IFileManagementService _fileManagementService;
        private readonly IPaginatedService _paginatedService;
        private static string host = string.Empty;
        #endregion

        #region Ctor
        public ImageManagementController(IFileManagementService fileManagementService,
            IConfiguration configuaration,
            IPaginatedService paginatedService)
        {
            _fileManagementService = fileManagementService;
            _paginatedService = paginatedService;
            host = configuaration.GetValue<string>("Host");
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(FileSearchModel searchModel)
        {
            var result = _fileManagementService.SearchImage(searchModel);
            ViewBag.Host = host;
            ViewBag.PaginatedModel = _paginatedService.GetMetaData(result.TotalItems, searchModel.PageIndex, searchModel.PageSize);
            ViewBag.SearchModel = JsonConvert.SerializeObject(searchModel);
            return PartialView("_ImageSearchResult", result.Data);
        }
        #endregion
    }
}