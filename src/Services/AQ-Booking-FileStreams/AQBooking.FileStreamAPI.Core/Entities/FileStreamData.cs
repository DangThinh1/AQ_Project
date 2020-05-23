using System;
using System.Collections.Generic;

namespace AQBooking.FileStreamAPI.Core.Entities
{
    public partial class FileStreamData
    {
        public int FileId { get; set; }
        public byte[] FileData { get; set; }
    }
}
