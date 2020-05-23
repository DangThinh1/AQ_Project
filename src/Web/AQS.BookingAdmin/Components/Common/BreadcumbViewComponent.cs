using AQS.BookingAdmin.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Components.Common
{
    public class BreadcrumbViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(BreadcumbModel model)
        {
            return View("Breadcrumb",model);
        }
    }
}
