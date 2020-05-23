using AQBooking.FileStream.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.FileRequest
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
