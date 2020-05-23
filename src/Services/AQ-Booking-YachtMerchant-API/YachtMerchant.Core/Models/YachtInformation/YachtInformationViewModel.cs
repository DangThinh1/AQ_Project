using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformation
{
    public class YachtInformationViewModel
    {
        public int Id { get; set; }
        public string ResourceKey { get; set; }
        public List<string> LanguagesSupported { get; set; }
        public int YachtFid { get; set; }
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
    }
}
