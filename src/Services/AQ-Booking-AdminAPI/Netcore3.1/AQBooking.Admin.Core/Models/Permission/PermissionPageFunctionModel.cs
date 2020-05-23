using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Permission
{
    public class PermissionPageFunctionModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleNameReskey { get; set; }
        public string Icon { get; set; }
        public List<PageModel> ListPage { get; set; }
    }

    public class PageModel
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageNameResKey { get; set; }
        public string Icon { get; set; }
        public List<FunctionModel> ListFunction { get; set; }
    }

    public class FunctionModel
    {
        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionNameReskey { get; set; }
    }
}

