namespace Identity.Core.Models.Usertokens
{
    public class UserTokenCreateModel
    {
        public string AccessToken { get; set; }
        public string ReturnUrl { get; set; }
    }
}
