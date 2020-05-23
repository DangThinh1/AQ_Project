using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace AQS.BookingMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
            }

            return PartialView("~/Views/Shared/User/_Register.cshtml", model);
        }
    }
}