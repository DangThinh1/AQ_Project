﻿using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantMenuPricings
    {
        public long Id { get; set; }
        public long MenuFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool HasPrice { get; set; }
        public bool Discounted { get; set; }
        public double? Price { get; set; }
        public double? OriginalPrice { get; set; }
        public double? DiscountedValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool Deleted { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual RestaurantMenus Menu { get; set; }
    }
}