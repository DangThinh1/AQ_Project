using System.Collections.Generic;

namespace AQConfigurations.Core.Models.Cities
{
    public class CityViewModel
    {
        public int CountryCode { get; set; }
        public string CityName { get; set; }
        public int CityCode { get; set; }
        public List<StateViewModel> StateLst { get; set; }
    }
}
