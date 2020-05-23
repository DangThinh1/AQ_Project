using System;

namespace AccommodationMerchant.Core.Models.HotelMerchantUsers
{
    public class HotelMerchantUserViewModel
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public Guid UserFid { get; set; }
        public int MerchantUserRoleFid { get; set; }
        public string MerchantUserRoleResKey { get; set; }
        public string UserEmail { get; set; }
        public string MerchantName { get; set; }
        public string UserName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
