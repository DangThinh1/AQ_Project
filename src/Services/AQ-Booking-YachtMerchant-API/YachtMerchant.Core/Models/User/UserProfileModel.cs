using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.User
{
    public class UserProfileModel
    {
        public string id { get; set; }
        public string uniqueId { get; set; }        
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string designation { get; set; }
        public string domainType { get; set; }
        public int ?  imageId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }       
        public string roleName { get; set; }
        public string roleId { get; set; }
        public int ? langId { get; set; }

    }
}
