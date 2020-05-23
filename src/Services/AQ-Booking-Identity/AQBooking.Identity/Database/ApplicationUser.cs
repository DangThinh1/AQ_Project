using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AQBooking.Identity.Database
{
    public class ApplicationUser : IdentityUser
    {
        //User info
        [StringLength(12)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UniqueId { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(30)]
        public string Designation { get; set; }
        [StringLength(30)]
        public string ProviderName { get; set; }
        [StringLength(50)]
        public string ProviderUserId { get; set; }
        public int? MerchantFid { get; set; }
        public string DomainFid { get; set; }
        public int? ImageId { get; set; }
        public int? LangId { get; set; }

        //Card Info
        [StringLength(30)]
        public string CardNumber { get; set; }
        [StringLength(30)]
        public string SecurityNumber { get; set; }
        [StringLength(30)]
        public string Expiration { get; set; }
        [StringLength(30)]
        public string CardHolderName { get; set; }
        [StringLength(30)]
        public int CardType { get; set; }

        //Address Info
        [StringLength(30)]
        public string Street { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(30)]
        public string State { get; set; }
        [StringLength(30)]
        public string Country { get; set; }
        [StringLength(30)]
        public string ZipCode { get; set; }

        //System
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        [StringLength(12)]
        public string RefreshToken { get; set; }

        public ApplicationUser():base()
        {
            Id = Guid.NewGuid().ToString();
            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            TwoFactorEnabled = false;
            LockoutEnabled = false;
            AccessFailedCount = 5;
            ProviderName = AuthProviderEnum.AqBooking.ToString();
            ProviderUserId = Id;
            Deleted = false;
            IsActivated = true;
            RefreshToken = UniqueIDHelper.GenarateRandomString(12);
            UniqueId = UniqueIDHelper.GenarateRandomString(12);
        }
    }
}
