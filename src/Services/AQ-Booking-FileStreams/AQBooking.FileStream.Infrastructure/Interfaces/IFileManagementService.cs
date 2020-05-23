using AQBooking.FileStream.Core.Models;
using AQBooking.FileStream.Core.Models.Common;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using AQBooking.FileStream.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Infrastructure.Interfaces
{
    public interface IFileManagementService
    {
        PagedList<FileViewModel> SearchFile(FileSearchModel model);
        PagedList<FileViewModel> SearchImage(FileSearchModel model);
        PagedList<FileViewModel> SearchBrochure(FileSearchModel model);
        PagedList<FileViewModel> SearchFileDeleted(FileSearchModel model);
        BasicResponse RemoveFile(int id);
        List<FileStreamInfo> GetAllFile();
        FileStatisticalModel GetFileStatistical();
    }
}
