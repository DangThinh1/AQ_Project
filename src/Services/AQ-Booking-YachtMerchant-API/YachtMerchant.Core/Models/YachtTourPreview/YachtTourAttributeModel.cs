using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourPreview
{
    public class YachtTourAttributeModel
    {
        public int? AttributeCategoryFid { get; set; }
        public int AttributeFid { get; set; }
        public string AttributeName { get; set; }
        public string Resourcekey { get; set; }
        public string AttributeValue { get; set; }
        public string IconClassCss { get; set; }
        public bool BasedAffective { get; set; }
    }
}
