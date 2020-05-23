namespace Identity.Infrastructure.Models
{
    public class JwtTokenCreateModel
    {
        public string UserId { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string RoleId { get; set; }
        public int ExpiredInMinute { get; set; }
        public string RoleGuidId { get; set; }
        public string MerchantFid { get; set; }
        public string RefreshToken { get; set; }
    }
}
