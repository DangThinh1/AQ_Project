using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class Yachts
    {
        public virtual YachtCounters Counter { get; set; }
        public virtual List<YachtAttributeValues> AttributeValues { get; set; }
        public virtual List<YachtCharterings> Charterings { get; set; }
        public virtual List<YachtFileStreams> FileStreams { get; set; }
        public virtual List<YachtInformations> Informations { get; set; }
        public virtual List<YachtOtherInformations> OtherInformations { get; set; }
        public virtual List<YachtNonOperationDays> NonOperationDays { get; set; }
        public virtual List<YachtPorts> Ports { get; set; }
        public virtual List<YachtPricingPlans> PricingPlans { get; set; }
        public virtual List<YachtAdditionalServiceControls> AdditionalServiceControls { get; set; }
        public virtual YachtMerchants Merchant { get; set; }

        public virtual YachtOptions Option { get; set; }

        public Yachts()
        {
            Counter = new YachtCounters();
            AttributeValues = new List<YachtAttributeValues>();
            Charterings = new List<YachtCharterings>();
            FileStreams = new List<YachtFileStreams>();
            Informations = new List<YachtInformations>();
            OtherInformations = new List<YachtOtherInformations>();
            NonOperationDays = new List<YachtNonOperationDays>();
            Ports = new List<YachtPorts>();
            PricingPlans = new List<YachtPricingPlans>();
            AdditionalServiceControls = new List<YachtAdditionalServiceControls>();
        }
    }
}
