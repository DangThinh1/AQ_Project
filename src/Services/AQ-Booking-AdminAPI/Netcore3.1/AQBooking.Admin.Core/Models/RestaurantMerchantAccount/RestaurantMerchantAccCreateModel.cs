using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.RestaurantMerchantAccount
{
    public class RestaurantMerchantAccCreateModel
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public string UserFid { get; set; }
        public string UserEmail { get; set; }
        public string MerchantName { get; set; }
        public string UserName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
