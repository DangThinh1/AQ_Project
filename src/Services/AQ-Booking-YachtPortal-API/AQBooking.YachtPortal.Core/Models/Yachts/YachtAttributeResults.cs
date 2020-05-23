using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtAttributeResults
    {
        public int AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeName { get; set; }

        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Remarks { get; set; }
        public string AttributeValue { get; set; }
    }
}
