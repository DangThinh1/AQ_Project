using AQBooking.YachtPortal.Core.Enum;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Implements.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.Common
{
    public class FileStreamService : ServiceBase, IFileStreamService
    {
        #region Field
        private readonly FileStreamApiUrl _fileStreamAPIURL;
        #endregion
        #region Ctor
        public FileStreamService(IOptions<FileStreamApiUrl> fileStreamApiUrlOption)
        {
            _fileStreamAPIURL = fileStreamApiUrlOption.Value;
        }
        #endregion
        #region Method
        ///// <summary>
        ///// Get File By Id
        ///// </summary>
        ///// <param name="id">File Id</param>
        ///// <returns>File url</returns>
        //public async Task<string> GetFileById(int id)
        //{
        //    return string.Empty;
        //}

        /// <summary>
        /// Get File By Id
        /// </summary>
        /// <param name="id">File Id</param>
        /// <param name="ratio">Image ratio</param>
        /// <returns>File url</returns>
        public async Task<string> GetFileById(int id, ThumbRatioEnum ratio)
        {
            return await GetFileUrl(id, (int)ratio);
        }

        /// <summary>
        /// Get File By Id And Type
        /// </summary>
        /// <param name="id">File id</param>
        /// <param name="typeId">File type id</param>
        /// <returns>File url</returns>
        public async Task<string> GetFileByIdAndType(int id, int typeId)
        {
            return await GetFileUrl(id, typeId);
        }

        /// <summary>
        /// Get File Url
        /// </summary>
        /// <param name="id">File id</param>
        /// <param name="typeId">File type id</param>
        /// <returns>File url</returns>
        private async Task<string> GetFileUrl(int id, int typeId)
        {
            var response = await _apiExcute.GetData<string>(
                $"{_baseFileStreamAPIUrl}{_fileStreamAPIURL.FileStream.GetFile}{id}/{typeId}",
                null,
                basicToken);

            return response.IsSuccessStatusCode
                ? response.ResponseData
                : "/images/no-image.png";
        }
        #endregion
    }
}
