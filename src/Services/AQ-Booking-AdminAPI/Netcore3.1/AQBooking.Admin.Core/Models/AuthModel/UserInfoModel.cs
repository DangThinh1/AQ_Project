using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.AuthModel
{
    public class UserInfoModel
    {
        public string UserId { get; set; }
        public string UniqueId { get; set; }
        public bool IsActivated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string RoleGuideId { get; set; }
        public int? ImageId { get; set; }
        public string DomainFid { get; set; }
        public int? LangId { get; set; }
    }
}
