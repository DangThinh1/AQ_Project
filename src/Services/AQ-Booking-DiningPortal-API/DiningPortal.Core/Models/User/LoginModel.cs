using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AQDiningPortal.Core.Models.User
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public int? Language { get; set; }
        public List<SelectListItem> ListLanguages { get; set; }

        public LoginModel()
        {
            Language = 1;

            ListLanguages = new List<SelectListItem>() {
                new SelectListItem("English", "1"),
                new SelectListItem("Chinese", "2"),
                new SelectListItem("Vietnamese", "3")
            };
        }
    }
}
