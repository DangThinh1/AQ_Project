﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Databases.EvisaEntities
{
    public partial class CountryVisaProcessOptionFees
    {
        public long Id { get; set; }
        public int CountryVisaProcessOptionFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double ProcessingFee { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool Deleted { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}