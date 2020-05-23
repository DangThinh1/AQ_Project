namespace Identity.Core.Portal.Models.SSOAuthentication
{
    public class SSOAuthenticationViewModel
    {
        public int Id { get; set; }
        public string DomainId { get; set; }
        public string RedirectUrl { get; set; }
        public string Token { get; set; }
        public bool Type { get; set; } = true;
        public string AuthId { get; set; }
    }
}