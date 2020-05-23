using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class CommonValues
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string ValueGroup { get; set; }
        public int? ValueInt { get; set; }
        public string ValueString { get; set; }
        public double? ValueDouble { get; set; }
        public string Text { get; set; }
        public string ResourceKey { get; set; }
        public double? OrderBy { get; set; }
    }
}