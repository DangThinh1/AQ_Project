using AQDiningPortal.Core.Models.PagingPortal;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.Restaurants
{
    public class SearchRestaurantWithMerchantIdModel: PagableModel
    {
        public string MerchantId { get; set; }
    }
}
