using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public class YachtAdditionalServiceDetailViewModel
    {
        public int AdditionalServiceFid { get; set; }
        public string AdditionalServiceName { get; set; }
        public int ProductFid { get; set; }
        public string ProductName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
        public List<YachtAdditionalServiceDetailModel> ListYachtAdditionalServiceDetail { get; set; }
    }
    public class YachtAdditionalServiceDetailModel
    {
        public int AdditionalServiceFid { get; set; }
        public int ProductFid { get; set; }
        public string ProductName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
