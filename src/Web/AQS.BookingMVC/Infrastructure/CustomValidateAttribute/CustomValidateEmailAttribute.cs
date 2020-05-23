using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AQS.BookingMVC.Infrastructure.CustomValidateAttribute
{
    public class CustomValidateEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var _partern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(value.ToString(), _partern, RegexOptions.IgnoreCase);
        }
    }
}
