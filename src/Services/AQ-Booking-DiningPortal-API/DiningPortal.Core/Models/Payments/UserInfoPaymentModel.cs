using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.Payments
{
    public class UserInfoPaymentModel
    {
        public string UserUniqueId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
    }
}
