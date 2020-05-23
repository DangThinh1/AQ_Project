using AQBooking.FileStreamAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStreamAPI.Core.Domain.Request
{
    public class FileDataModel
    {
        public string FileName { get; set; }
        public string FileExtention { get; set; }
        public byte[] FileData { get; set; }
        public FileTypeEnum FileTypeFid { get; set; }
        public string DomainId { get; set; }
        public string FolderId { get; set; }
    }
}
