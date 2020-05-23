using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Common.FileStream
{
    public class FileUploadResponseModel
    {
        public string FileName { get; set; }
        public double FileSize { get; set; }
        public int FileId { get; set; }
        public string DataImage { get; set; }
    }
}
