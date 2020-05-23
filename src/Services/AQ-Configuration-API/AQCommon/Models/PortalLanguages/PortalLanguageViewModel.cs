namespace AQConfigurations.Core.Models.PortalLanguages
{
    public class PortalLanguageViewModel
    {
        public int Id { get; set; }
        public string PortalUniqueId { get; set; }
        public int DomainPortalFid { get; set; }
        public int LanguageFid { get; set; }
        public string LanguageName { get; set; }
    }
}
