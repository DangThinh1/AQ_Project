using System;
using System.Collections.Generic;

namespace AQBooking.FileStream.Infrastructure.Entities
{
    public partial class FileStreamInfo
    {
        public int FileId { get; set; }
        public Guid? UniqueCode { get; set; }
        public string UniqueId { get; set; }
        public int? FileTypeFid { get; set; }
        public string OriginalName { get; set; }
        public string FileName { get; set; }
        public string Seoname { get; set; }
        public string Path { get; set; }
        public string PathThumb12 { get; set; }
        public string PathThumb14 { get; set; }
        public string PathThumb16 { get; set; }
        public string PathThumb18 { get; set; }
        public string FileExtentions { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadedDateUtc { get; set; }
        public string UploadedBy { get; set; }
        public bool IsNew { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsPortraitImage { get; set; }
    }
}
