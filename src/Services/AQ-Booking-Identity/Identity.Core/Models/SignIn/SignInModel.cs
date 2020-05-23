namespace Identity.Core.Models.SignIn
{
    public partial class SignInModel : SignInModelBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
