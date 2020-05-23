using AQBooking.FileStream.Core.Models;
using AQBooking.FileStream.Core.Models.Common;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.FileStreamWeb.Interfaces
{
    public interface IFileManagementService
    {
        PagedListClientModel<FileViewModel> SearchFile(FileSearchModel model);
        PagedListClientModel<FileViewModel> SearchImage(FileSearchModel model);
        PagedListClientModel<FileViewModel> SearchBrochure(FileSearchModel model);
        PagedListClientModel<FileViewModel> SearchFileDeleted(FileSearchModel model);
        FileStatisticalModel FileStatistical();
        bool DeleteFile(int id);
        BasicResponse RemoveFile(int id);
        bool RestoreFile(int id);
    }
}
