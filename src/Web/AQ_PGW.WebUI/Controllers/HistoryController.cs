using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AQ_PGW.WebUI.Helper;
using AQ_PGW.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AQ_PGW.WebUI.Controllers
{
    public class HistoryController : Controller
    {
        ITransactionsServiceRepository _TransactionsService;
        public HistoryController(ITransactionsServiceRepository TransactionsService)
        {
            _TransactionsService = TransactionsService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(int? page, HistoryModel dt, string typePage, string sortBy)
        {
            switch (typePage.ToUpper())
            {
                case "UNPAID":
                    var PartialUnPaid = PageUnPaid(page, dt, sortBy);
                    return PartialUnPaid;
                case "PENDING":
                    var PartialPending = PagePending(page, dt, sortBy);
                    return PartialPending;
                case "PAID":
                    var PartialPaid = PagePaid(page, dt, sortBy);
                    return PartialPaid;
                case "ORDER COMPLETE":
                    var PartialComplete = PageComplete(page, dt, sortBy);
                    return PartialComplete;
                default:
                    var PartialAll = PageAll(page, dt, sortBy);
                    return PartialAll;
            }
            //var response = Helper.RestAPI.SendGetRequest("api/Transaction/GetAllTrans?Today=" + dt.Today.ToShortDateString() + "&FromDate=" + dt.Day.ToShortDateString());
            //var result = Helper.HelperCommon.DeserializeObject<SearchTransactionsModel>(response.Content);
            //if (result == null)
            //{
            //    return PartialView("~/Views/History/IndexPartial.cshtml", null);
            //}
            //return PartialView("~/Views/History/IndexPartial.cshtml", result);
        }

        [HttpPost]
        public IActionResult PageUnPaid(int? page, HistoryModel dt, string sortBy)
        {
            var response = Helper.RestAPI.SendGetRequest($"api/Transaction/GetPageTrans?page={page?.ToString()}&Today={dt.Today.ToShortDateString()}&FromDate={ dt.Day.ToShortDateString()}&sortBy={sortBy}&type=UNPAID");
            var result = Helper.HelperCommon.DeserializeObject<PageModel<TransactionModel>>(response.Content);
            return PartialView("~/Views/History/_Search.cshtml", result);
        }

        [HttpPost]
        public IActionResult PagePending(int? page, HistoryModel dt, string sortBy)
        {
            var response = Helper.RestAPI.SendGetRequest($"api/Transaction/GetPageTrans?page={page?.ToString()}&Today={dt.Today.ToShortDateString()}&FromDate={ dt.Day.ToShortDateString()}&sortBy={sortBy}&type=PEDDING");
            var result = Helper.HelperCommon.DeserializeObject<PageModel<TransactionModel>>(response.Content);
            return PartialView("~/Views/History/_Search.cshtml", result);
        }

        [HttpPost]
        public IActionResult PagePaid(int? page, HistoryModel dt, string sortBy)
        {
            var response = Helper.RestAPI.SendGetRequest($"api/Transaction/GetPageTrans?page={page?.ToString()}&Today={dt.Today.ToShortDateString()}&FromDate={ dt.Day.ToShortDateString()}&sortBy={sortBy}&type=PAID");
            var result = Helper.HelperCommon.DeserializeObject<PageModel<TransactionModel>>(response.Content);
            return PartialView("~/Views/History/_Search.cshtml", result);
        }

        [HttpPost]
        public IActionResult PageComplete(int? page, HistoryModel dt, string sortBy)
        {
            var response = Helper.RestAPI.SendGetRequest($"api/Transaction/GetPageTrans?page={page?.ToString()}&Today={dt.Today.ToShortDateString()}&FromDate={ dt.Day.ToShortDateString()}&sortBy={sortBy}&type=COMPLETE");
            var result = Helper.HelperCommon.DeserializeObject<PageModel<TransactionModel>>(response.Content);
            return PartialView("~/Views/History/_Search.cshtml", result);
        }

        [HttpPost]
        public IActionResult PageAll(int? page, HistoryModel dt, string sortBy)
        {
            var response = Helper.RestAPI.SendGetRequest($"api/Transaction/GetPageTrans?page={page?.ToString()}&Today={dt.Today.ToShortDateString()}&FromDate={ dt.Day.ToShortDateString()}&sortBy={sortBy}&type=ALL");
            var result = Helper.HelperCommon.DeserializeObject<PageModel<TransactionModel>>(response.Content);
            return PartialView("~/Views/History/_Search.cshtml", result);
        }
    }
}