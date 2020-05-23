using System.Collections.Generic;
using AQBooking.YachtPortal.Core.Models.YachtCounters;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtInformations;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlans;
using AQBooking.YachtPortal.Core.Models.YachtDestinationPlans;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtDetailViewModel
    {
        public YachtDetailModel Yacht { get; set; }
        public YachtCounterViewModel Counter { get; set; }
        public YachtInformationViewModel Information { get; set; }
        public List<YachtFileStreamViewModel> FileStreams { get; set; }
        public List<YachtPricingPlanViewModel> PricingPlans { get; set; }
        public List<YachtAttributeValueViewModel> AttributeValues { get; set; }

        public YachtDetailViewModel()
        {
            Yacht = new YachtDetailModel();
            Counter = new YachtCounterViewModel();
            Information = new YachtInformationViewModel();
            FileStreams = new List<YachtFileStreamViewModel>();
            PricingPlans = new List<YachtPricingPlanViewModel>();
            AttributeValues = new List<YachtAttributeValueViewModel>();
        }
    }
}
