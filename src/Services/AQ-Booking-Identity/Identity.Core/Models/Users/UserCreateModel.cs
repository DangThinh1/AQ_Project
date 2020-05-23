using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models.Users
{
    public class UserCreateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }

        public string RoleId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public int? MerchantFid { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
