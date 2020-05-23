using System;

namespace Identity.Core.Models.Usertokens
{
    public class UserTokenViewModel
    {
        public string Id { get; set; }
        public Guid UserFid { get; set; }
        public string AccessToken { get; set; }
        public string ReturnUrl { get; set; }
    }
}
