using AQBooking.FileStream.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.FileRequest
{
    public class FileSearchModel : PagableModel
    {
        public int FileId { get; set; }
        public string UniqueId { get; set; }
        public string OriginalName { get; set; }
        public string FileExtension { get; set; }
        public string UploadedDateUTC { get; set; }
        public bool Deleted { get; set; }
    }
}
