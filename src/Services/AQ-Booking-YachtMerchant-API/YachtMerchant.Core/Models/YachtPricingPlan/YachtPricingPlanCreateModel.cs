using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtPricingPlan
{
    public class YachtPricingPlanCreateModel
    {
        public YachtPricingPlanCreateModel()
        {
            lstPricingPlanDetail = new List<YachtPricingPlanDetailCreateModel>();
            yachtPricingInfo = new List<YachtPricingPlanInformationCreateModel>();
        }
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int YachtFid { get; set; }
        public int PricingCategoryFid { get; set; }
        public string PricingCategoryResKey { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool IsRecurring { get; set; }
        public string PlanName { get; set; }
        public bool BasedPortLocation { get; set; }
        public int YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int PricingPlanFid { get; set; }
        public int PricingTypeFid { get; set; }
        public string PricingTypeResKey { get; set; }
        public bool ContactOwner { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int LanguageFid { get; set; }        
        public string PackageInfo { get; set; }
        public List<YachtPricingPlanDetailCreateModel> lstPricingPlanDetail { get; set; }
        public List<YachtPricingPlanInformationCreateModel> yachtPricingInfo { get; set; }


    }
}
