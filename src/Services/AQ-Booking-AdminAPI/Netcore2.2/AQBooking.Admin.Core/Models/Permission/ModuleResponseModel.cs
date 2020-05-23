using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Permission
{
    public class ModuleResponseModel
    {
        public int ModuleID { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameResKey { get; set; }
        public string ControllerName { get; set; }
        public string Icon { get; set; }
    }
}
