using AQBooking.FileStream.Core.Models.Common;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using AQBooking.FileStream.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Infrastructure.Interfaces
{
    public interface IImageManagementService
    {
        List<FileStreamInfo> GetAllImage();
        bool DeleteFile(int id);
    }
}
