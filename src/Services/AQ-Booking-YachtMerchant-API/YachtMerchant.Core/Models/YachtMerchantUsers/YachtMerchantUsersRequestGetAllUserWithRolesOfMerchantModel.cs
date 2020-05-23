using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantUsers
{
    public class YachtMerchantUsersRequestGetAllUserWithRolesOfMerchantModel
    {
        public int MerchantId { get; set; }
        public int Role { get; set; }
    }
}
