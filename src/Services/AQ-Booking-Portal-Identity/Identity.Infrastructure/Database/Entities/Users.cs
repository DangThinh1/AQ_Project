using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Helpers;
using Identity.Infrastructure.Enums;

namespace Identity.Infrastructure.Database.Entities
{
    public class Users : IdentityUser<int>
    {
        public Guid UserId { get; set; }
        public string UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public int ImageId { get; set; }
        public int LangId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TokenEffectiveDate { get; set; }
        public long TokenEffectiveTimeStick { get; set; }
        public string RefreshToken { get; set; }
        public string DisplayName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Title { get; set; }
        public int? MerchantFid { get; set; }
        public string DomainFid { get; set; }

        public virtual List<UserRoles> UserRoles { get; set; }
        public virtual Roles Roles {
            get => UserRoles.Select(k => k.Role).FirstOrDefault() ?? new Roles() { Id = 0, Name=string.Empty, DomainFid=string.Empty };
        }

        public Users(string email, string firstName, string lastName) : base()
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            InitCommonProperties();
        }

        private void InitCommonProperties()
        {
            UserId = Guid.NewGuid();
            UniqueId = UniqueIDHelper.GenarateRandomString(12);
            NormalizedEmail = Email.ToUpper();
            UserName = Email;
            NormalizedUserName = Email.ToUpper();
            CreatedBy = UserId;
            CreatedDate = DateTime.Now;
            Deleted = false;
            IsActivated = true;
            SecurityStamp = Guid.NewGuid().ToString("D");
            LockoutEnabled = true;
            DomainFid = string.Empty;
            MerchantFid = 0;
            LangId = 1;
            DisplayName = $"{FirstName} {LastName}";
            Title = string.Empty;
            Birthday = null;
            TokenEffectiveDate = DateTime.Now;
            TokenEffectiveTimeStick = 0;
            TokenEffectiveDate = CreatedDate;
        }
    }
}
