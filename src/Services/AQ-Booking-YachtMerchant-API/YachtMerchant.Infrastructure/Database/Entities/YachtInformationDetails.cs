﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class YachtInformationDetails
    {
        public long Id { get; set; }
        public string UniqueId { get; set; }
        public int InformationFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}