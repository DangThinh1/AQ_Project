using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Permission
{
    public class AuthorizationModuleModel
    {
        public int ModuleId { get; set; }
        public string DisplayName { get; set; }
        public string ControllerName { get; set; }
        public bool? Active { get; set; }
        public int? OrderBy { get; set; }
        public string Icon { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool? IsShow { get; set; }
    }
}
