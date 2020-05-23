using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtAttributes
{
    public class YachtAttributeViewModel
    {
        public int Id { get; set; }
        public string AttributeName { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Remarks { get; set; }
    }
}
