using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using System.Collections.Generic;
namespace AQBooking.YachtPortal.Core.Models.YachtPricingPlans
{
    public class YachtPricingPlanViewModel
    {
        public string Id { get; set; }
        public int PricingCategoryFid { get; set; }
        public string PricingCategoryResKey { get; set; }
        public string PlanName { get; set; }
        public bool BasedPortLocation { get; set; }
        public string YachtPortName { get; set; }       
        public string Remark { get; set; }
        public List<YachtPricingPlanDetailViewModel> Details { get; set; }        
    }    
}
