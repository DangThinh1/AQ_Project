namespace AQDiningPortal.Core.Models.User
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string DomainType { get; set; }
        public int? ImageId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int? RoleId { get; set; }
        public int? LangId { get; set; }

    }
}
