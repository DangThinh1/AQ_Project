using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class Countries
    {
        [Key]
        public int CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
    }
}
