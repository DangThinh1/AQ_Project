using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantUsers
{
    public class YachtMerchantUsersCreateModel
    {
        public int MerchantFid { get; set; }
        public Guid UserFid { get; set; }
        public int MerchantUserRoleFid { get; set; }
        public string MerchantUserRoleResKey { get; set; }
        public string UserEmail { get; set; }
        public string MerchantName { get; set; }
        public string UserName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
