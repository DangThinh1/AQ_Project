using AQBooking.FileStreamAPI.Core.Domain.Request;
using AQBooking.FileStreamAPI.Core.Domain.Response;
using AQBooking.FileStreamAPI.Core.Entities;
using AQBooking.FileStreamAPI.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.FileStreamAPI.Services.FileService
{
    public interface IFileService
    {
        Task<FileStreamInfo> GetFileInfoById(int id);
        Task<byte[]> GetFileDataById(int id);
        Task<int> InsertFileInfo(FileStreamInfo fileInfo);
        Task<int> InsertFileData(FileStreamData fileData);
        Task<object> UploadFile(IFormFile file, FileTypeEnum fileType, string domainFid, string merchantFid);
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
        /// Delete file by file Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteFileById(int id);

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
