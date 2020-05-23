using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Database.Entities
{
    public class SSOAuthentication
    {
        public int Id { get; set; }
        public string DomainId { get; set; }
        public string Token { get; set; }
        public string RedirectUrl { get; set; }
        public bool Type { get; set; } = true;
        public string UserUid { get; set; }
        public string AuthId { get; set; }
    }
}