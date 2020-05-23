using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Common
{
    public class BreadcumbModel
    {
        public BreadcumbModel()
        {

        }
        public BreadcumbModel(string title, Dictionary<string, string> breadcrumbs)
        {
            PageTitle = title;
            Breadcrumbs = breadcrumbs;
        }
        public string PageTitle { get; set; }
        public Dictionary<string,string> Breadcrumbs { get; set; }
    }
}
