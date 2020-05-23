using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Permission
{
    public class FunctionResponseModel
    {
        public int FuntionId { get; set; }
        public string FunctionName { get; set; }
        public bool? Active { get; set; }
    }
}
