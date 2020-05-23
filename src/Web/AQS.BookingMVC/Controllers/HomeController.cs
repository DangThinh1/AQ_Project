using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Interfaces;
using AQS.BookingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AQS.BookingMVC.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkContext _workContext;

        public HomeController(ILogger<HomeController> logger,
            IWorkContext workContext)
        {
            _logger = logger;
            _workContext = workContext;
        }

        [Route("",Name = "HomePage")]
        public IActionResult Index()
        {
            if(_workContext.IsComingSoon)
            {
                return RedirectToRoute("TravelBlog", new {lang="en-us",lang_id=CommonValueConstant.DEFAULT_LANGUAGE_ID });
            }
            //ok o
            return View();
        }

     
        public IActionResult Privacy()
        {
            // edit code privacy
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
