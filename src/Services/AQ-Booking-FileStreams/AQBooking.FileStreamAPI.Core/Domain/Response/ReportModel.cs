using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStreamAPI.Core.Domain.Response
{
    public class ReportModel
    {
        public DateTime DateReport { get; set; }
        public Guid UserExcuteId { get; set; }
        public int TotalItems { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFail { get; set; }

        public IEnumerable<ReportLine> ReportLines { get; set; }
    }

    public class ReportLine
    {
        public DateTime ReportTime { get; set; }
        public int FileId { get; set; }
        public int FileType { get; set; }
        public string FileName { get; set; }
        public string FileEx { get; set; }
        public int FileSize { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
