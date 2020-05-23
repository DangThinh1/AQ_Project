using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingMVC.Areas.Yacht.Controllers
{
    public class YachtHomeController : BaseYachtController
    {
        public IActionResult Index()
        {
            return RedirectToAction("YachtSearchIndex", "YachtSearch");
        }
    }
}