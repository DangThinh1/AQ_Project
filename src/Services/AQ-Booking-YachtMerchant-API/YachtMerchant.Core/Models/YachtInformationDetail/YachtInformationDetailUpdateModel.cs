using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformationDetail
{
    public class YachtInformationDetailUpdateModel
    {
        public int Id { get; set; }
        public int InformationId { get; set; }
        public int FileTypeFID { get; set; }
        public int FileStreamFID { get; set; }
        public int LanguageFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
