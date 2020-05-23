namespace Identity.Web.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public bool IsValidEmail { get; set; }
        public string CallBackUrl { get; set; }

        public string LoginResKey { get; set; }
        public string LoginMessage { get; set; }
    }
}