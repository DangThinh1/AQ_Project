using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class Yachts
    {
        public int Id { get; set; }
        public int MerchantFid { get; set; }
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public int CharterTypeFid { get; set; }
        public string CharterTypeResKey { get; set; }
        public int CharterCategoryFid { get; set; }
        public string CharterCategoryResKey { get; set; }
        public string BuilderName { get; set; }
        public string DesignerName { get; set; }
        public double? LengthMeters { get; set; }
        public double? BeamMeters { get; set; }
        public double? DraftMeters { get; set; }
        public int YachtTypeFid { get; set; }
        public string YachtTypeResKey { get; set; }
        public int HullTypeFid { get; set; }
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
        public int? DestinationTypeFid { get; set; }
        public string DestinationTypeResKey { get; set; }
        public bool HasDestinationPlans { get; set; }
        public bool ActiveForOperation { get; set; }
        public bool Deleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual YachtCounters Counters { get; set; }
        public virtual IEnumerable<YachtAttributeValues> AttributeValues { get; set; }
        public virtual IEnumerable<YachtCharterings> Charterings { get; set; }
        public virtual IEnumerable<YachtDestinationPlans> DestinationPlans { get; set; }
        public virtual IEnumerable<YachtFileStreams> FileStreams { get; set; }
        public virtual IEnumerable<YachtInformations> Informations { get; set; }
        public virtual IEnumerable<YachtOtherInformations> OtherInformations { get; set; }
        public virtual IEnumerable<YachtNonOperationDays> NonOperationDays { get; set; }
        public virtual IEnumerable<YachtPorts> Ports { get; set; }
    }
}