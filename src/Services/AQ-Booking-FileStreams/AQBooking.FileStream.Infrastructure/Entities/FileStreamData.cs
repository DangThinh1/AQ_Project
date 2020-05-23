using System;
using System.Collections.Generic;

namespace AQBooking.FileStream.Infrastructure.Entities
{
    public partial class FileStreamData
    {
        public int FileId { get; set; }
        public byte[] FileData { get; set; }
    }
}
