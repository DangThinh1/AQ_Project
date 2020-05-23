using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.AuthModel
{
    public class LoginResultModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string UserRole { get; set; }
        public double Expired { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
    }
}