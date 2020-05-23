using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Permission
{
    public class UpdatePageFunctionModel
    {
        public int PageId { get; set; }
        public List<int> ListFunctionId { get; set; }
    }
}
