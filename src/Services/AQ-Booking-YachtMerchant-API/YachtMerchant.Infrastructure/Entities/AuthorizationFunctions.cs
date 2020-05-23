using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class AuthorizationFunctions
    {
        public int FuntionId { get; set; }
        public string FunctionName { get; set; }
        public bool? Active { get; set; }
    }
}