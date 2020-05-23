using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingMVC.Areas.Hotel.Controllers
{
    [Area("Hotel")]
    public class HotelHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}