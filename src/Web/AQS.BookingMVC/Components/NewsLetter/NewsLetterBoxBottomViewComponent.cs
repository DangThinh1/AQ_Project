using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Components.NewsLetter
{
    public class NewsLetterBoxBottomViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("NewsLetterBoxBottom");
        }
    }
}
