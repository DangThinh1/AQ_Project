﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class YachtPricingPlanDetails
    {
        public long Id { get; set; }
        public int PricingPlanFid { get; set; }
        public int PricingTypeFid { get; set; }
        public string PricingTypeResKey { get; set; }
        public bool ContactOwner { get; set; }
        public double Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}