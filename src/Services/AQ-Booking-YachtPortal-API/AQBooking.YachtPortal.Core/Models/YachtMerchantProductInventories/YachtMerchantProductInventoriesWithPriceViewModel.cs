using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories
{
    public class YachtMerchantProductInventoriesWithPriceViewModel
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }  
        public string ProductCode { get; set; }
        public string ProductName { get; set; }           
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
       
    }
}
