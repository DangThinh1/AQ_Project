using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtOtherInformation
{
    public class YachtOtherInformationAddOrUpdateModel
    {
        public int Id { get; set; }
        public int YachtFid { get; set; }
        public int InfoTypeFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public string ActivatedDate { get; set; }
    }
}
