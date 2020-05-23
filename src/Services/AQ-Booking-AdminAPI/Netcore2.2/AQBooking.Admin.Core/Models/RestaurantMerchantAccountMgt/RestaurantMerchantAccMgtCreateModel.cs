using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.RestaurantMerchantAccountMgt
{
    public class RestaurantMerchantAccMgtCreateModel
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public string AqadminUserFid { get; set; }
        public string UserEmail { get; set; }
        public string MerchantName { get; set; }
        public string UserName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
