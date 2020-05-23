using AQBooking.YachtPortal.Core.Models.YachtAttributes;
using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValueCharterPrivateGeneralViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CharterTypeReskey { get; set; }
        public int CharterCategoryFid { get; set; }
        public int CharterTypeFid { get; set; }
        public int Cabins { get; set; }
        public double LengthMeters { get; set; }
        public double BeamMeters { get; set; }
        public int MaxPassenger { get; set; }
        public int OvernightPassengers { get; set; }
        public double MaxSpeed { get; set; }        
        public string EngineGenerators { get; set; }
        public List<YachtAttributeValueViewModel> AttributeValues { get; set; } = new List<YachtAttributeValueViewModel>();
    }
}
