using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.FileResponse
{
    public class FileStatisticalModel
    {
        public int TotalFiles { get; set; }
        public int TotalImageFiles { get; set; }
        public int TotalBrochureFiles { get; set; }
        public int TotalDeletedFiles { get; set; }
    }
}
