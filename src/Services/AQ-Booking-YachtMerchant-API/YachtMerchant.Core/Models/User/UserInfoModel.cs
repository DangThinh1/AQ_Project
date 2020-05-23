using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.User
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public int PublicId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderName { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string Expiration { get; set; }
        public string CardHolderName { get; set; }
        public int CardType { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessToken { get; set; }
        public int? LangId { get; set; }
        public int? ImageId { get; set; }
    }

    public class UserBasicInfoModel
    {
        public string uniqueId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string designation { get; set; }
        public int? merchantFid { get; set; }
        public int? domainFid { get; set; }
        public int? imageId { get; set; }
        public int langId { get; set; }
        public string street { get; set; }
        public string Street { get; set; }
        public string city { get; set; }
        public string state  { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
        public bool isActivated { get; set; }
        public string refreshToken { get; set; }
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string roleName { get; set; }
        public int roleId { get; set; }
        public string roleGuidId { get; set; }


    }
}
