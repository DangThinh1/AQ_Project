using System.Diagnostics;
using Identity.Web.Models;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Identity.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var cook = HttpContext.Request.Cookies;
            //var option = new CookieOptions();
            //option.IsEssential = true;
            //option.Expires = DateTime.Now.AddMinutes(10);
            //Response.Cookies.Append("AQTest", "true", option);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message="")
        {
            if (!string.IsNullOrEmpty(message))
            {
                var model = new ErrorViewModel()
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = message ?? string.Empty
                };
                return View(model);
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}