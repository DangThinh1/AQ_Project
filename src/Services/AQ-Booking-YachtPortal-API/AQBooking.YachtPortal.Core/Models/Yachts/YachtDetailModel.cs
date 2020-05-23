using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtDetailModel
    {
        public string Name { get; set; }
        public string CharterTypeResKey { get; set; }
        public string CharterCategoryResKey { get; set; }
        public string BuilderName { get; set; }
        public string DesignerName { get; set; }
        public double? LengthMeters { get; set; }
        public double? BeamMeters { get; set; }
        public double? DraftMeters { get; set; }
        public string YachtTypeResKey { get; set; }
        public string HullTypeResKey { get; set; }
        public string Year { get; set; }
        public string EngineGenerators { get; set; }
        public string Material { get; set; }
        public string Stabilisers { get; set; }
        public double? CruisingSpeed { get; set; }
        public double? MaxSpeed { get; set; }
        public string Refrigerator { get; set; }
        public bool Ac { get; set; }
        public bool Acsurcharge { get; set; }
        public string Electricity { get; set; }
        public bool WaterMaker { get; set; }
        public double? WaterCapacityLtrs { get; set; }
        public double? FuelCapacityLtrs { get; set; }
        public double? CruisingFuel { get; set; }
        public double? CruisingRange { get; set; }
        public int CrewMembers { get; set; }
        public int Cabins { get; set; }
        public int MaxPassengers { get; set; }
        public int DayTripPassengers { get; set; }
        public int OvernightPassengers { get; set; }
        public string MoreDetailNote { get; set; }
        public string CountryRegistered { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string DestinationTypeResKey { get; set; }
        public bool HasDestinationPlans { get; set; }
    }
}
