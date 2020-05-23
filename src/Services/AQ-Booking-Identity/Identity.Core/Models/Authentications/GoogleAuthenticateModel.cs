using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Core.Models.Authentications
{
   public class GoogleAuthenticateModel
    {
        [Required]
        public string GoogleId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string ImageUrl { get; set; }

    }
}
