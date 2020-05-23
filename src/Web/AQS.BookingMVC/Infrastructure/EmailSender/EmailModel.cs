using AQS.BookingMVC.Infrastructure.CustomValidateAttribute;
using AQS.BookingMVC.Infrastructure.ReCaptCha;
using System.ComponentModel.DataAnnotations;

namespace AQS.BookingMVC.Infrastructure.EmailSender
{
    public class EmailModel : GoogleReCaptchaModelBase
    {
        public string YourName { get; set; }
        public string Tilte { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [CustomValidateEmail]
        public string Email { get; set; }
        public string Content { get; set; }
    }
}
