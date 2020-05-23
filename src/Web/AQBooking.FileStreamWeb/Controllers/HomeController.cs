using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AQBooking.FileStreamWeb.Models;
using Microsoft.Extensions.Configuration;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStreamWeb.Interfaces;
using AQBooking.FileStreamWeb.Helpers.AQPagination;

namespace AQBooking.FileStreamWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IFileManagementService _fileManagementService;
        private readonly IPaginatedService _paginatedService;
        private static string host = string.Empty;
        #endregion

        #region Ctor
        public HomeController(IFileManagementService fileManagementService, 
            IConfiguration configuaration,
            IPaginatedService paginatedService)
        {
            _fileManagementService = fileManagementService;
            _paginatedService = paginatedService;
            host = configuaration.GetValue<string>("Host");
        }
        #endregion

        #region Methods
        [HttpGet]
        public IActionResult Index()
        {
            var result = _fileManagementService.FileStatistical();
            return View(result);
        }

        [HttpPost]
        public IActionResult DeleteFile(int id)
        {
            var result = _fileManagementService.DeleteFile(id);
            return Json(result);
        }

        [HttpPost]
        public IActionResult RemoveFile(int id)
        {
            var result = _fileManagementService.RemoveFile(id);
            return Json(result);
        }

        [HttpPost]
        public IActionResult RestoreFile(int id)
        {
            var result = _fileManagementService.RestoreFile(id);
            return Json(result);
        }
        #endregion
    }
}
