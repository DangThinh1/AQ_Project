using APIHelpers;
using APIHelpers.Request;
using System.Threading.Tasks;
using APIHelpers.Response;
using System;
using AQS.BookingAdmin.Models.Common.FileStream;
using Microsoft.Extensions.Options;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQBooking.Admin.Core.Enums;
using AQS.BookingAdmin.Services.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using AQS.BookingAdmin.Infrastructure.Constants;
using System.IO;
using System.Linq;

namespace AQS.BookingAdmin.Services.Implements.Common
{
    public class FileStreamService : BaseService, IFileStreamService
    {
        #region Fields
        private readonly string _baseFileStreamAPIUrl;
        private readonly IAQFileProvider _aQFileProvider;
        #endregion

        #region Ctor
        public FileStreamService(IOptions<ApiServer> apiServerOptions,
            IOptions<AdminPortalApiUrl> _adminPortalApiUrlOptions,
             IAQFileProvider aQFileProvider
            ) :base()
        {
            _aPIExcute = new APIExcute(AuthenticationType.Basic);
            _baseFileStreamAPIUrl = apiServerOptions.Value.AQFileStreamApi.GetCurrentServer();
            _aQFileProvider = aQFileProvider;
            
        }
        #endregion

        #region Methods
        public async Task<BaseResponse<FileUploadResponseModel>> UploadFile(FileUploadRequestModel model)
        {
            try
            {
               
                var req = new BaseRequest<FileUploadRequestModel>(model);
                var res = await _aPIExcute.PostData<FileUploadResponseModel, FileUploadRequestModel>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.File}", HttpMethodEnum.POST, req,_basicAPIToken);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<FileUploadResponseModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<FileUploadResponseModel>> UploadFileData(FileDataUploadRequestModel model)
        {
            try
            {
                var req = new BaseRequest<FileDataUploadRequestModel>(model);
                var res = await _aPIExcute.PostData<FileUploadResponseModel, FileDataUploadRequestModel>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.FileData}", HttpMethodEnum.POST, req, _basicAPIToken);
                if (res.IsSuccessStatusCode)
                    return res;
                return null;
            }
            catch (Exception ex)
            {
                return BaseResponse<FileUploadResponseModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<string> GetFileById(int id)
        {
            var resUrl = string.Empty;
            var res = await _aPIExcute.GetData<string>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.File}/{id}", null, _basicAPIToken);
            if (res.IsSuccessStatusCode)
                resUrl = res.ResponseData;
            return resUrl;
        }

        public async Task<BaseResponse<FileInfoResponseModel>> GetFileInfo(int fileId)
        {
            try
            {
                var res = await _aPIExcute.GetData<FileInfoResponseModel>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.FileInfo}/{fileId}", null, _basicAPIToken);
                if (res.IsSuccessStatusCode)
                    return res;
                return BaseResponse<FileInfoResponseModel>.Success(new FileInfoResponseModel());
            }
            catch (Exception ex)
            {
                return BaseResponse<FileInfoResponseModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<string> GetFileById(int id, ThumbRatioEnum ratio)
        {
            try
            {
                string resUrl = string.Empty;
                var res = await _aPIExcute.GetData<string>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.File}/{id}/{(int)ratio}", null, _basicAPIToken);
                if (res.IsSuccessStatusCode)
                    resUrl = res.ResponseData;
                return resUrl;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BaseResponse<bool>> DeleteFile(int id)
        {
            try
            {
                var res = await _aPIExcute.PostData<bool, int>($"{_baseFileStreamAPIUrl}{_adminPortalApiUrl.FileStreamAPI.File}/{id}", HttpMethodEnum.DELETE, null, _basicAPIToken);
                if (res.IsSuccessStatusCode)
                    return res;
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<(string fileName, string fileUrl)> UploadTempFile(IFormFile formFile,string folder)
        {
            string tempFolderName = $"{CommonConstant.FILE_TEMP_FOLDER}/{folder}";
            string tempFolderPath = _aQFileProvider.GetAbsolutePath(tempFolderName);
            if (!_aQFileProvider.DirectoryExists(tempFolderPath))
                _aQFileProvider.CreateDirectory(tempFolderPath);
            string fileName =$"{Guid.NewGuid()}____{formFile.FileName}";
            string filePath = $"{tempFolderPath}/{fileName}";
            if (!_aQFileProvider.FileExists(filePath))
            {
                using (Stream f = File.OpenWrite(filePath))
                {
                    await formFile.CopyToAsync(f);
                    
                };
            }
            return (fileName, $"/{tempFolderName}/{fileName}");
        }

        public (byte[] fileBinary, string filePath,string fileName) GetTempFile(string fileName, string folder)
        {
            string originalFileName = fileName.Split("____").Last();
            string filePath = $"{CommonConstant.FILE_TEMP_FOLDER}/{folder}/{fileName}";
            string fullFilePath = _aQFileProvider.GetAbsolutePath(filePath);
            if (!_aQFileProvider.FileExists(fullFilePath))
                return (null, filePath,fileName);
            var fileBinary = _aQFileProvider.ReadAllBytes(fullFilePath);
            return (fileBinary, filePath, originalFileName);
        }
        public void DeleteFileTemp(string fileName, string folder)
        {
            string filePath = $"{CommonConstant.FILE_TEMP_FOLDER}/{folder}/{fileName}";
            string fullFilePath = _aQFileProvider.GetAbsolutePath(filePath);
            if (_aQFileProvider.FileExists(fullFilePath))
                _aQFileProvider.DeleteFile(fullFilePath);
          
        }
        #endregion
    }
}
