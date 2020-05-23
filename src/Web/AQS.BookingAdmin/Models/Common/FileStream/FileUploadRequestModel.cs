using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Models.Common.FileStream
{
    public class FileUploadRequestModel
    {
        public IFormFile File { get; set; }
        public int FileTypeFid { get; set; }
        public string DomainId { get; set; }
        public string FolderId { get; set; }
    }
}
