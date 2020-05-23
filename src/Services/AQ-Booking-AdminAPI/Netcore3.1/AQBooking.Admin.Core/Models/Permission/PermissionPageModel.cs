using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Permission
{
    public class PermissionPageModel
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageNameResKey { get; set; }
        public int ModuleId { get; set; }
        public int? ParentId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
    }
}
