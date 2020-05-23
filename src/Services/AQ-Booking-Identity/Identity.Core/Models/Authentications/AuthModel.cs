using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models.Authentications
{
    public class AuthModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class FacebookAuthModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccessToken { get; set; }
    }
}
