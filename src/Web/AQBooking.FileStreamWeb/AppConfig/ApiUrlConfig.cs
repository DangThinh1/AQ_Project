using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.FileStreamWeb.AppConfig
{
    public class ApiUrlConfig
    {
        public FileManagementUrl FileManagementUrl { get; set; }
        public FileHandleUrl FileHandleUrl { get; set; }
    }

    public class FileManagementUrl
    {
        public string Index { get; set; }
        public string ImageManagement { get; set; }
        public string BrochureManagement { get; set; }
        public string FileDeleted { get; set; }
        public string FileStatistical { get; set; }
    }

    public class FileHandleUrl
    {
        public string DeleteFile { get; set; }
        public string RestoreFile { get; set; }
    }
}
