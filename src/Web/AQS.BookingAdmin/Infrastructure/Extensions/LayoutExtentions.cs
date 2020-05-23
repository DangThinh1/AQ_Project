using AQS.BookingAdmin.Models.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Infrastructure.Extensions
{
    public static class LayoutExtentions
    {
        public static Task<IHtmlContent> Breadcrumb(this IViewComponentHelper componentHelper, string title, Dictionary<string, string> hrefAndName=null)
        {
            if (hrefAndName == null)
                hrefAndName = new Dictionary<string, string>();
           return componentHelper.InvokeAsync("Breadcrumb", new BreadcumbModel(title, hrefAndName));
        }
        public static IHtmlContent Help(this IHtmlHelper html,string help)
        {
            return html.Raw("<i data-toggle=\"tooltip\" title=\"" + help + "\" class=\"fa fa-question-circle text-sm\"></i>");
        }
    }
}
