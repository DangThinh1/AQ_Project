namespace Identity.Web.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}