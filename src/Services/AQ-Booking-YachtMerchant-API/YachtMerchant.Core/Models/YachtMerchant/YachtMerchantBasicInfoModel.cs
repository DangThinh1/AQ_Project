using System;

namespace YachtMerchant.Core.Models.YachtMerchant
{
    public class YachtMerchantBasicInfoModel
    {
        public int MerchantId { get; set; }
        public string MerchantUniqueId { get; set; }
        public string MerchantName { get; set; }
        public string Country { get; set; }
    }

    public class YachtMerchantProfileModel
    {
        public int id { get; set; }
        public string uniqueId { get; set; }
        public int zoneFid { get; set; }
        public string merchantName { get; set; }
        public string address1 { get; set; }
        public object address2 { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public object zipCode { get; set; }
        public string contactNumber1 { get; set; }
        public object contactNumber2 { get; set; }
        public string emailAddress1 { get; set; }
        public object emailAddress2 { get; set; }
        public int accountSize { get; set; }
        public object remark { get; set; }
        public DateTime expiredDate { get; set; }
        public string createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime lastModifiedDate { get; set; }
    }
}
