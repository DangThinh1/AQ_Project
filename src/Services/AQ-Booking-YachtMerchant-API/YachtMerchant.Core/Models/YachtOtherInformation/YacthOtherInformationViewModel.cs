using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtOtherInformation
{
    public class YacthOtherInformationViewModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int YachtFid { get; set; }
        public int InfoTypeFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }

        public string ResourceKey { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
