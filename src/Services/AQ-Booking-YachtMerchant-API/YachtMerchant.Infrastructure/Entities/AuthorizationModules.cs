using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class AuthorizationModules
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