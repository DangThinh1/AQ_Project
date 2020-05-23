using AQBooking.FileStream.Infrastructure.Entities;
using AQBooking.FileStream.Core.Enums;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AQBooking.FileStream.Core.Models;

namespace AQBooking.FileStream.Infrastructure.Interfaces
{
    public interface IFileHandleService
    {
        Task<FileStreamInfo> GetFileInfoById(int id);
        Task<byte[]> GetFileDataById(int id);
        Task<int> InsertFileInfo(FileStreamInfo fileInfo);
        Task<int> InsertFileData(FileStreamData fileData);
        Task<object> UploadFile(IFormFile file, FileTypeEnum fileType, string domainId = "", string folderId = "");
        Task<object> UploadFileData(FileDataModel model);

        /// <summary>
        /// Get file url by fileId 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetFileById(int id);

        /// <summary>
        /// Get file url by fileId and ratio of the image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        Task<string> GetFileById(int id, ThumbRatioEnum ratio);

        /// <summary>
        /// Get file brochure by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileBrochureModel> GetFileBrochureById(int id);

        /// <summary>
        /// Delete file by file Id (set deleted = true)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteFileById(int id);

        /// <summary>
        /// Only find and remove files in directory by file path (file path get from file id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BasicResponse RemoveFileInDirectory(int id);

        /// <summary>
        /// Restore all file
        /// </summary>
        /// <returns></returns>
        Task<RestoreFileResponse> RestoreFile(bool includeFileDeleted);

        /// <summary>
        /// Restore file by fileId
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<bool> RestoreFile(int fileId, bool includeFileDeleted);

        /// <summary>
        /// Restore file by fileId and ratio of image
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        Task<bool> RestoreFile(int fileId, ThumbRatioEnum ratio, bool includeFileDeleted);

        /// <summary>
        /// Inject file id and get file infomation
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        FileStreamInfo GetFileInfo(int fileId);
    }
}
