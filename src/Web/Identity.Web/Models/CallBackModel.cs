using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Web.Models
{
    public class CallBackModel
    {
        public string AccessToken { get; set; }
        public string ReturnUrl { get; set; }
        public string CallBackUrl { get; set; }
    }
}
