using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AQ_PGW.WebUI.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AQ_PGW.WebUI.Controllers
{
    public class CardController : Controller
    {
        public IActionResult Index(string cusID)
        {
            cusID = "cus_Eue2iFwuOLZHmJ";
            var result = RestAPI.SendPostRequest($"api/Transaction/GetCardByCustomer?cusID=" + cusID, null);
            var customerItems = HelperCommon.DeserializeObject<Stripe.Customer>(result.Content);
            return View(customerItems);
        }
        [HttpPost]
        public IActionResult Delete(string cusID, string cardID)
        {
            var result = RestAPI.SendPostRequest($"api/Transaction/DeleteCard?cusID=" + cusID + "&cardID=" + cardID, null);
            var customerRemoveItems = HelperCommon.DeserializeObject<Stripe.Card>(result.Content);
            if (customerRemoveItems.Deleted == true)
            {
                var resultCus = RestAPI.SendPostRequest($"api/Transaction/GetCardByCustomer?cusID=" + cusID, null);
                var customerItems = HelperCommon.DeserializeObject<Stripe.Customer>(resultCus.Content);
                return PartialView("~/Views/Card/_Default.cshtml", customerItems);
            }
            return PartialView("~/Views/Card/_Default.cshtml", new Stripe.Customer() );
        }
        [HttpPost]
        public IActionResult EditDefault(string cusID, string cardID)
        {
            var result = RestAPI.SendPostRequest($"api/Transaction/DefaultCard?cusID=" + cusID + "&cardID=" + cardID, null);
            var customerEditItems = HelperCommon.DeserializeObject<Stripe.Customer>(result.Content);
            return PartialView("~/Views/Card/_Default.cshtml", customerEditItems);
        }
        public IActionResult CreateCard(string tokenCard, string cusID)
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTokenCard(string tokenCard, string cusID)
        {
            //tokenCard = "tok_1EZx3JDA5slZiHRBUi7BcAa8";
            //cusID = "cus_Eue2iFwuOLZHmJ";
            var result = RestAPI.SendPostRequest($"api/Transaction/CreateCard?tokenCard=" + tokenCard + "&cusID=" + cusID, null);
            var createCard = HelperCommon.DeserializeObject<Stripe.Customer>(result.Content);
            return Json("Create Card Success");
        }
    }
}