using System;

namespace Identity.Infrastructure.Database.Entities
{
    public class UserTokens
    {
        public string Id { get; set; }
        public Guid UserFid { get; set; }
        public string AccessToken { get; set; }
        public string ReturnUrl { get; set; }
        public bool Deleted { get; set; }
    }
}
