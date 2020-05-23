using AQBooking.Admin.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Media
{
    public class UploadFileModel
    {
        public byte[] FileData { get; set; }
        public FileTypeEnum FileType { get; set; }
        public string FileName { get; set; }
    }
}
