using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingMVC.Areas.Dining.Controllers
{
    public class DiningHomeController : BaseHotelController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}