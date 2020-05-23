using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Subscriber;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using AQS.BookingMVC.Infrastructure.Extensions;
using ClosedXML.Excel;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingAdmin.Controllers
{
    public class EmailSubcriberController : BaseController
    {

        #region Fields
        private readonly IEmailSubcriberService _emailSubcriberService;
        #endregion

        #region Ctor
        public EmailSubcriberController(IEmailSubcriberService emailSubcriberService)
        {
            _emailSubcriberService = emailSubcriberService;
        }
        #endregion

        #region Methods
        #region Actions
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            var model = new SubscriberSearchModel();
            return View(model);
        }
        public async Task<IActionResult> ListData(SubscriberSearchModel model)
        {
            var data = await _emailSubcriberService.SearchEmailSubcriber(model);
            ViewBag.lstCountSub = data.ResponseData.Data.Count();
            return PartialView("_EmailSubcriberListTable", data.GetDataResponse());

        }
        [HttpGet]
        public IActionResult Export(string DateFrom, string DateTo)
        {
            SubscriberSearchModel model = new SubscriberSearchModel();
            string fileName = string.Empty;
            if (string.IsNullOrEmpty(DateFrom) && string.IsNullOrEmpty(DateTo))
                return Content("");
            model.CreatedDateFrom = DateTime.Parse(DateFrom);
            model.CreatedDateTo = DateTime.Parse(DateTo);
            var res = _emailSubcriberService.GetLstSubcriToExport(model).Result;

            if (res != null && res.Count() > 0)
                fileName = ExportEmailSubToExcel(res);

            if (!string.IsNullOrEmpty(fileName))
                return Json(new { fileName = fileName });
            else
                return Json(new { fileName = fileName, errorMessage = "No data to export" });
        }

        [HttpGet]
        public IActionResult DownloadFileExcel(string fileName, string dateFrom, string dateTo)
        {
            var streamF = HttpContext.Session.Get("fileStream");
            string nameExcel = $"Email_Subcriber_{dateFrom}_{dateTo}.xlsx";
            return File(streamF, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nameExcel);
        }

        #endregion
        #region Utilities
        private string ExportEmailSubToExcel(List<SubscriberViewModel> model)
        {
            //try
            //{
            if (model != null & model.Count() > 0)
            {
                //ClosedXML
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[5]
                {
                        new DataColumn("Email", typeof(string)),
                        new DataColumn("Source URL", typeof(string)),
                        new DataColumn("Module Name", typeof(string)),
                        new DataColumn("Activated", typeof(string)),
                        new DataColumn("Created Date", typeof(string)),
                });
                //Add DataTable to List Data
                foreach (var item in model)
                    dt.Rows.Add(item.Email, item.SourceUrl, item.ModuleName, item.IsActivated, item.CreatedDate);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Sheet1");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        HttpContext.Session.Set("fileStream", stream.ToArray());
                        return "fileStream";
                    }
                }
            }
            else
                return null;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        #endregion
        #endregion

    }
}