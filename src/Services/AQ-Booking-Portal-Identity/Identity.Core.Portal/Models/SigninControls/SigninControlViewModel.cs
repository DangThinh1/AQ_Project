namespace Identity.Core.Portal.Models.SigninControls
{
    public class SigninControlViewModel
    {
        public int Id { get; set; }
        public string CurrentDomainUid { get; set; }
        public string CurrentDomainName { get; set; }
        public string ToGoDomainUid { get; set; }
        public string ToGoDomainName { get; set; }
        public string ToGoDomainCallbackUrl { get; set; }
    }
}