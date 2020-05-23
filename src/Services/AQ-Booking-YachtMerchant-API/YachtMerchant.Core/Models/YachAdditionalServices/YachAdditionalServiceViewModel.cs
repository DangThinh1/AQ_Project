using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public class YachAdditionalServiceViewModel
    {
        public int Id { get; set; }
        public string AdditonalServiceTypeResKey { get; set; }
        public int AdditonalServiceTypeFid { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public int CountServiceDetail { get; set; }
        public int CountServiceControl { get; set; }
    }
}
