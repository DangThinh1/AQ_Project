using AQBooking.FileStreamAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStreamAPI.Services.ImageManagement
{
    public interface IImageManagementService
    {
        List<FileStreamInfo> GetAllImage();
        bool DeleteFile(int id);
    }
}
