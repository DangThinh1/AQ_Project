using AQS.BookingMVC.Infrastructure.CustomValidateAttribute;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AQS.BookingMVC.Models.Payment
{
    public class PaymentStripInfo
    {
        [Required]
        public string name { get; set; }
        [Required]
        [CreditCard]
        public string cardNumber { get; set; }
        [Required]
        public int exp_Month { get; set; }
        [Required]
        public int exp_Year { get; set; }
        [Required]
        public string cvc { get; set; }
        public int paymentmethod { get; set; }
    }
    public class BookingInfo
    {
        [Required]
        public string NameOfUser { get; set; }
        [EmailAddress]
        [CustomValidateEmail]
        public string EmailOfUser { get; set; }
        [Phone]
        public string ContactNo { get; set; }
        public string IdOfUser { get; set; }
        public int IsEmailExist { get; set; }
    }

    public class SaveBookingRequestModel
    {
        public BookingRequestModel RedisCartRequestModel { get; set; }
        public BookingInfo BookingRequestModel { get; set; }
    }

    public class BookingTotalFee
    {
        public double dbTotalFee { get; set; }
        public double dbTotalRepaid { get; set; }
        public string DisplayTotalFee { get; set; }
        public string DisplayTotalRepaid { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
    }
    public class BookingRequestModel
    {
        public string HashKey { get; set; }
        public string Key { get; set; }
        public string Domain { get; set; }
        public List<string> itemList { get; set; }
    }
}
