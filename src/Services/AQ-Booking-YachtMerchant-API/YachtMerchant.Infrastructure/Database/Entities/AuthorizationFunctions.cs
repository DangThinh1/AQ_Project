using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class AuthorizationFunctions
    {
        public int FuntionId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionNameResKey { get; set; }
        public bool? Active { get; set; }
    }
}