using System.Threading.Tasks;
using APIHelpers.Response;
using AQBooking.Admin.Core.Enums;
using AQS.BookingAdmin.Models.Common.FileStream;
using Microsoft.AspNetCore.Http;

namespace AQS.BookingAdmin.Services.Interfaces.Common
{
    public interface IFileStreamService
    {
        /// <summary>
        /// Select file and upload to server
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<FileUploadResponseModel>> UploadFile(FileUploadRequestModel model);

        /// <summary>
        /// Upload file image by binary
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<FileUploadResponseModel>> UploadFileData(FileDataUploadRequestModel model);

        /// <summary>
        /// Inject file id and get url of image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetFileById(int id);

        /// <summary>
        /// Inject file id and list url by ratio
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        Task<string> GetFileById(int id, ThumbRatioEnum ratio);

        /// <summary>
        /// Inject file id to delete file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteFile(int id);

        /// <summary>
        /// Get all file info by fileId
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<BaseResponse<FileInfoResponseModel>> GetFileInfo(int fileId);

        /// <summary>
        /// Upload a file to temp folder
        /// </summary>
        /// <param name="formFile">FormFile</param>
        /// <param name="folder">Folder name </param>
        /// <returns>File name, and fileurl</returns>
        Task<(string fileName, string fileUrl)> UploadTempFile(IFormFile formFile,string folder);
        /// <summary>
        /// Get temp file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="folder">folder</param>
        /// <returns></returns>
        (byte[] fileBinary, string filePath, string fileName) GetTempFile(string fileName, string folder);
        /// <summary>
        /// delete temp file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="folder">Folder</param>
        void DeleteFileTemp(string fileName, string folder);


    }
}
