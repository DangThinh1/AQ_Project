using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YachtMerchant.Core.Models.User
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public int Language { get; set; } = 1;
        public List<SelectListItem> ListLanguages { get; set; }

        public LoginModel()
        {
            Language = 1;
        }
    }
}
