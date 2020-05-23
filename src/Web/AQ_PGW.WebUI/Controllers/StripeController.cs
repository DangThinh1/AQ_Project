using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.WebUI.Helper;
using AQ_PGW.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using AES_Encryption;
using AQ_PGW.Common.Important;
using Newtonsoft.Json;
using static AQ_PGW.Common.DTO;

namespace AQ_PGW.WebUI.Controllers
{
    public class StripeController : Controller
    {

        ITransactionsServiceRepository _TransactionsService;
        public StripeController(ITransactionsServiceRepository TransactionsService)
        {
            _TransactionsService = TransactionsService;
        }
        public IActionResult Index(string value)
        {
            var trans = _TransactionsService.GetTransactionById(value);
            return View(trans);
        }
        [HttpPost]
        public IActionResult PostData([FromBody]Transactions value)
        {
            if (value != null)
            {
                var trans = _TransactionsService.GetTransactionById(value.ID.ToString());
                if (trans == null)
                {
                    return Json("Fail");
                }

                var rescoure = RestAPI.SendPostRequest("api/Transaction/ProcessPayment", value);

                return Json(rescoure);
            }
            return Json("Fail");
        }
        public IActionResult Create()
        {
            return View();
        }

        [ActionName("InsertData")]
        public IActionResult Create(SubmitForm rescoure)
        {
            EncryptHelper EncryptHelper = new EncryptHelper(CommonConst.API_ENCRYPT_KEY);
            string data = JsonConvert.SerializeObject(rescoure);
            string dataEncrypt = EncryptHelper.EncryptString(data);
            return RedirectToAction("Payment", new { token = dataEncrypt });

        }

        public IActionResult Payment(string token)
        {
            try
            {
                EncryptHelper EncryptHelper = new EncryptHelper(CommonConst.API_ENCRYPT_KEY);
                token = token.Replace(" ", "+");
                string dataDecrypt = EncryptHelper.DecryptString(token);
                var rescoure = JsonConvert.DeserializeObject<SubmitForm>(dataDecrypt);

                Transactions trans = new Transactions()
                {
                    ID = Guid.NewGuid(),
                    OrderId = rescoure.OrderId,
                    CreatedDate = DateTime.Now,
                    OrderAmount = rescoure.Amount,
                    Description = rescoure.Decription,
                    PaymentMethod = rescoure.PaymentMethod,
                    BackUrl = rescoure.BackUrl,
                    Currency = rescoure.Currency,
                };
                var response = RestAPI.SendPostRequest("api/Transaction/RequestPayment", trans);

                dynamic jsonDeserialize = JsonConvert.DeserializeObject(response.Content);
                if (jsonDeserialize.statusCode.ToString() == "2" || jsonDeserialize.statusCode.ToString() == "0")
                {
                    return Redirect($"{trans.BackUrl}?status={jsonDeserialize.message.ToString()}&errorCode=2");
                }
                else
                    return View("Index", trans);


            }
            catch (Exception ex)
            {
                var ErrorViewModel = new ErrorViewModel()
                {
                    RequestId = "Error Submit",
                };
                return View("Error", ErrorViewModel);
            }

        }
        //[HttpPost]
        //public IActionResult Success([FromBody]APIResponseData status)
        //{
        //    //var jsonInfo = JsonConvert.DeserializeObject(status);
        //    return View("Success", status);
        //}

        public IActionResult Success(string status)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                RequestId = status,
            };
            //var jsonInfo = JsonConvert.DeserializeObject(status);
            return View(ErrorViewModel);
        }
    }
}