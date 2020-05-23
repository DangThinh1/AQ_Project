using System;

namespace Identity.Core.Models.Users
{
    public class UserProfileUpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string DomainFid { get; set; }
        public int? MerchantFid { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
