namespace Identity.Core.Models.Authentications
{
    public class AuthenticateViewModel
    {
        public string AccountTypeFid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string Expired { get; set; }
        public string UserRole { get; set; }
        public string UniqueId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenEffectiveDate { get; set; }
        public string TokenEffectiveTimeStick { get; set; }
    }
}
