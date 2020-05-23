using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class AuthorizationRoles
    {
        public int DesignationId { get; set; }
        public int PageFid { get; set; }
        public int FunctionFid { get; set; }
    }
}