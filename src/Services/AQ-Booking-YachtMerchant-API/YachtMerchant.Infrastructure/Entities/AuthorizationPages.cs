using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class AuthorizationPages
    {
        public int PageId { get; set; }
        public int? ModuleFid { get; set; }
        public string PageName { get; set; }
        public string WebName { get; set; }
        public int? OrderBy { get; set; }
        public int? ParentFid { get; set; }
        public string Tooltip { get; set; }
        public string Icon { get; set; }
        public bool? Active { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string LinkVideo { get; set; }
        public int? OrderByVideo { get; set; }
    }
}