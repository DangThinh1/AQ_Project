using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AQS.BookingMVC.Infrastructure.ReCaptCha
{
    public class GoogleReCaptchaModelBase
    {
        [GoogleReCaptchaValidation]
        [BindProperty(Name = "g-recaptcha-response")]
        public String GoogleReCaptchaResponse { get; set; }
        public bool IsCapchaValid { get;internal set; }
    }
}
