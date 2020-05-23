namespace AQConfigurations.Core.Models.CommonValues
{
    public class CommonValueCreateModel
    {
        public string ValueGroup { get; set; }
        public int? ValueInt { get; set; }
        public string ValueString { get; set; }
        public double? ValueDouble { get; set; }
        public string Text { get; set; }
        public string ResourceKey { get; set; }
        public double? OrderBy { get; set; }
    }
}