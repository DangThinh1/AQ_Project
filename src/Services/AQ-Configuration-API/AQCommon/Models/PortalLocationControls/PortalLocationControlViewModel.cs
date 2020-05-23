namespace AQConfigurations.Core.Models.PortalLocationControls
{
    public partial class PortalLocationControlViewModel
    {
        public int Id { get; set; }
        public string PortalUniqueId { get; set; }
        public int DomainPortalFId { get; set; }
        public string CountryName { get; set; }
        public int CountryCode { get; set; }
        public string CityName { get; set; }
        public int CityCode { get; set; }
        public string CssClass { get; set; }
        public int FileStreamFId { get; set; }
        public bool IsExclusive { get; set; }
        public float OrderBy { get; set; }
    }
}