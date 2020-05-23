using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQS.BookingMVC.Models.YachtBooking;
using Microsoft.AspNetCore.Mvc;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Interfaces;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQS.BookingMVC.Areas.Yacht.Controllers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQS.BookingMVC.Services.Interfaces.Yatch;

namespace AQS.BookingMVC.Areas.Yatch.Controllers
{
    public class YachtBookingController : BaseYachtController
    {
        #region Fields
        private readonly IYachtBookingService _yachtBookingService;
        private readonly IYatchService _yatchService;
        #endregion

        #region Ctor
        public YachtBookingController(IYachtBookingService yachtBookingService, IYatchService yatchService)
        {
            _yachtBookingService = yachtBookingService;
            _yatchService = yatchService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load form yacht book by Tuan Kiet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult YachtBookingForm(YachtBookingModel model)
        {
            var lstAttribute = new List<string>()
            {
                "Bathroom","Bedding"
            };
            var responseDetail = _yatchService.GetYatchDetail(model.YachtID.ToString(), 1, true, lstAttribute).Result;
            model.Details = responseDetail.GetDataResponse();
            return PartialView("_YachtBookingForm", model);
        }
        /// <summary>
        /// Save Yacht Book by TuanKiet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveYachtBook(YachtBookingModel model)
        {
            if (ModelState.IsValid)
            {
                //HttpContext.Session.SetObject("YachtCart", model);
                //RedisCachesModel redisModel = new RedisCachesModel();
                //_yachtBookingService.SaveYachtBookingtoRedis(redisModel);
            }
            return Ok();
        }
        /// <summary>
        /// Load yacht book detail by Tuan Kiet
        /// </summary>
        /// <returns></returns>
        public IActionResult Checkout()
        {
            YachtCharteringViewModel res = new YachtCharteringViewModel();
            if (WorkContext.IsAuthentication && HttpContext.Session.Keys.Count() > 0)
            {
                res = _yachtBookingService.GetCharterByReservationEmail(WorkContext.CurentUser.Email);
                //ViewBag.modelReservation = HttpContext.Session.GetObject<YachtBookingModel>("YachtCart");
            }
            return View("Checkout", res);
        }

        #endregion
    }
}