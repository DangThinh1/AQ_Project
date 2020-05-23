using System;

namespace YachtMerchant.Core.Models.YachtMerchantInformation
{
    public class YachtMerchantInformationAddOrUpdateModel
    {
        public long Id { get; set; }
        public int MerchantFid { get; set; }
        public int InformationFid { get; set; }
        public int LanguageFid { get; set; }
        public string ResourceKey { get; set; }
        public int FileTypeFID { get; set; }
        public string Remark { get; set; }
        public int FileStreamFID { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public string FileName { get; set; }
        public string File { get; set; }
    }
}