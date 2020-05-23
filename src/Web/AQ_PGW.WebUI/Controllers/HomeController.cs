using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AQ_PGW.WebUI.Models;
using AQ_PGW.WebUI.Helper;
using Newtonsoft.Json;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.Model;

namespace AQ_PGW.WebUI.Controllers
{
    public class HomeController : Controller
    {
        ITransactionsServiceRepository _TransactionsService;
        public HomeController(ITransactionsServiceRepository TransactionsService)
        {
            _TransactionsService = TransactionsService;
        }
        public IActionResult Index()
        {
            //DateTime dt = new DateTime(2018, 12, 14);
            //var result = RestAPI.SendGetRequest($"api/Transaction/SearchPayment?Today={DateTime.Now.ToShortDateString()}&FromDate={DateTime.Now.AddDays(2).ToShortDateString()}");

            //var obj = HelperCommon.DeserializeObjects<Transactions>(result.Content);
            return View();
        }

        [HttpGet]
        public IActionResult PageTrans(int? page, string sortOrder)
        {
            var viewModel = GetListPage(page, sortOrder);

            if (viewModel == null)
            {
                viewModel = new PageModel<TransactionModel>();
            }
            return PartialView("~/Views/Home/PatialViewHome/_IndexPage.cshtml", viewModel);
        }

        public PageModel<TransactionModel> GetListPage(int? page, string orderBy)
        {
            //var dummyItems = Enumerable.Range(1, 150).Select(x => "Item " + x);
            var result = RestAPI.SendGetRequest($"api/Transaction/GetTransactions?page={page?.ToString()}&sortBy={orderBy}");
            var dummyItems = HelperCommon.DeserializeObject<PageModel<TransactionModel>>(result.Content);
            return dummyItems;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
