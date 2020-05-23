using System;
using System.Collections.Generic;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class Currencies
    {
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public string ResourceKey { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
