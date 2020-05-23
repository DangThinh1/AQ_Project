using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductInventories
{
    public class YachtMerchantProductInventorySearchModel: SearchModel
    {
        public int MerchantId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; } 
        public int GSTTTypeId { get; set; }
        public int PriceTypeId { get; set; }
        public int ItemUnitId { get; set; }
    }
}
