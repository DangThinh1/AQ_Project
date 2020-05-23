using System;
using System.Collections.Generic;

namespace AQBooking.FileStreamAPI.Core.Entities
{
    public partial class FileType
    {
        public int FileTypeId { get; set; }
        public Guid? UniqueCode { get; set; }
        public string FileTypeName { get; set; }
        public bool? Deleted { get; set; }
    }
}
