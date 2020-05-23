using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtInformations;
using AQBooking.YachtPortal.Core.Models.YachtPorts;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtPrivateCharterViewModel
    {
        public string ID { get; set; }        
        public string City { get; set; }
        public string UniqueID { get; set; }
        public string Country { get; set; }
        public string CharterTypeReskey { get; set; }        
        public int CharterCategoryFid { get; set; }        
        public int CharterTypeFid { get; set; }
        public int Cabins { get; set; }
        public double LengthMeters { get; set; }
        public double BeamMeters { get; set; }
        public int MaxPassenger { get; set; }
        public int OvernightPassengers { get; set; }
        public long? CharteringFId { get; set; }
        public double MaxSpeed { get; set; }
        public string Name { get; set; }
        public string EngineGenerators { get; set; }     
        public int? FileStreamFid { get; set; }
        public string FileThumbUrl { get; set; }
    }
}
