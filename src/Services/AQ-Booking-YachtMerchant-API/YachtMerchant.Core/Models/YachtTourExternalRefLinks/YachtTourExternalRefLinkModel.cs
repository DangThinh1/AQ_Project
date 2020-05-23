using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtExternalRefLinks
{
    public class YachtTourExternalRefLinkModel
    {
        public long Id { get; set; }
        public string YachtTourFid { get; set; }
        public int LinkTypeFid { get; set; }
        public string LinkTypeResKey { get; set; }
        public string Name { get; set; }
        public string UrlLink { get; set; }
        public string Remark { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
