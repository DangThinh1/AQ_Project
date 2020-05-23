using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Permission
{
    public class PageResponseModel
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public int ModuleId { get; set; }
        public int? ParentId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
    }
}
