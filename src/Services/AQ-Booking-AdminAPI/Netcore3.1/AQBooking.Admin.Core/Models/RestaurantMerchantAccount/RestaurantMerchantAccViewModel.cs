using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.RestaurantMerchantAccount
{
    public class RestaurantMerchantAccViewModel : RestaurantMerchantAccCreateModel
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
