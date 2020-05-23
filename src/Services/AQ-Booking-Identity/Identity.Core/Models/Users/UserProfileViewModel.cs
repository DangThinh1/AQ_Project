using System;
using System.Collections.Generic;

namespace Identity.Core.Models.Users
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; }
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string RefreshToken { get; set; }
        public int ImageId { get; set; }
        public int? LangId { get; set; }
        public bool IsActivated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string DomainFid { get; set; }
        public int? MerchantFid { get; set; }
        public string DisplayName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Title { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<PaymentCardViewModel> PaymentCards { get; set; }

        public UserProfileViewModel()
        {
            PaymentCards = new List<PaymentCardViewModel>()
            {
                new PaymentCardViewModel(){
                    Id="1",
                    SecurityNumber = "9843",
                    CardHolderName = $"{LastName} {FirstName}",
                    Expiration = "07/2019",
                    CardType = 1
                },
                new PaymentCardViewModel(){
                    Id="1",
                    SecurityNumber = "9843",
                    CardHolderName = $"{LastName} {FirstName}",
                    Expiration = "07/2019",
                    CardType = 1
                },
                new PaymentCardViewModel(){
                    Id="1",
                    SecurityNumber = "9843",
                    CardHolderName = $"{LastName} {FirstName}",
                    Expiration = "07/2019",
                    CardType = 1
                },
                new PaymentCardViewModel(){
                    Id="1",
                    SecurityNumber = "9843",
                    CardHolderName = $"{LastName} {FirstName}",
                    Expiration = "07/2019",
                    CardType = 1
                },
             };
        }
    }
}
