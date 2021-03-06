﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace AQBooking.Admin.Infrastructure.Databases.EvisaEntities
{
    public partial class VisaApplicationDocuments
    {
        public long Id { get; set; }
        public long ApplicationFid { get; set; }
        public string UniqueId { get; set; }
        public string OriginalName { get; set; }
        public string FileName { get; set; }
        public string FileExtentions { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadedDateUtc { get; set; }
        public string UploadedBy { get; set; }
        public string HashKey { get; set; }
        public bool IsEncryptedPassword { get; set; }
        public string PasswordUnlock { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}