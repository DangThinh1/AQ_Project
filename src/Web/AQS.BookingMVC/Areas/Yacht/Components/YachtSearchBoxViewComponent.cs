using AQBooking.YachtPortal.Core.Models.Yachts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Areas.Yacht.Components
{
    public class YachtSearchBoxViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(YachtSearchModel model)
        {
            return View("YachtSearchBox",model);
        }
    }
}
