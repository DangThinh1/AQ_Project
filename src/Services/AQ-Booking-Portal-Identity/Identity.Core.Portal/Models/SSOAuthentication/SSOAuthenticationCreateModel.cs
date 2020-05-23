using System;

namespace Identity.Core.Portal.Models.SSOAuthentication
{
    public class SSOAuthenticationCreateModel
    {
        public string DomainId { get; set; }
        public string RedirectUrl { get; set; }
        public string Token { get; set; }
        public bool Type { get; set; } = true;
        public string UserUid { get; set; }
    }
}