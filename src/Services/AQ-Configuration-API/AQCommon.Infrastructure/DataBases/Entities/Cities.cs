using System.ComponentModel.DataAnnotations;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class Cities
    {
        [Key]
        public int CityCode { get; set; }
        public int CountryCode { get; set; }
        public string CityName { get; set; }
    }
}
