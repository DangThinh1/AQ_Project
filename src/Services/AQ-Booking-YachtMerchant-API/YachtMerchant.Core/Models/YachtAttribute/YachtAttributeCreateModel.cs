using System.ComponentModel.DataAnnotations;

namespace YachtMerchant.Core.Models.YachtAttribute
{
    public class YachtAttributeCreateModel
    {
        public int? AttributeCategoryFid { get; set; }
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public string ResourceKey { get; set; }
        public string IconCssClass { get; set; }
        public string Remarks { get; set; }
        public bool? IsDefault { get; set; }
        public double? OrderBy { get; set; }
    }
}
