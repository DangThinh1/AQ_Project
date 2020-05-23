using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Users
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UniqueId { get; set; }     
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetFullName()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }
}
