using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using AQBooking.FileStream.Infrastructure.Interfaces;
using AQBooking.FileStream.Core.Enums;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using Microsoft.AspNetCore.Cors;
using AQBooking.FileStream.Core.Constants;

namespace AQBooking.FileStreamAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class FileHandleController : ControllerBase
    {
        #region Fields
        private readonly IFileHandleService _fileService;
        private readonly IConfiguration _configuaration;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private static string restoreKey = string.Empty;
        #endregion

        #region Ctor
        public FileHandleController(
            IFileHandleService fileService,
            IConfiguration configuaration)
        {
            _fileService = fileService;
            _configuaration = configuaration;
            restoreKey = configuaration.GetValue<string>("RestoreKey");
        }
        #endregion

        #region Method
        /// <summary>
        /// Choose a file to upload
        /// </summary>
        /// <param name="file">Select and upload file from formData</param>
        /// <param name="fileTypeFid">File type is enum</param>
        /// <param name="domainId">DomainId is unieuqId of domain portal in common values, ex: UEJTHY87UJMK </param>
        /// <param name="folderId">FolderId is uniqueId of restaurant, yacht... , ex: HTYJIU57UYJN</param>
        /// <returns></returns>
        [HttpPost]
        [Route("File")]        
        public async Task<IActionResult> UploadFile(IFormFile file, FileTypeEnum fileTypeFid, string domainId, string folderId)
        {
            try
            {
                var result = await _fileService.UploadFile(file, fileTypeFid, domainId, folderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Select multiple files to upload
        /// </summary>
        /// <param name="files">Is IEnumerable of file, you can use the postman to test</param>
        /// <param name="fileTypeFid">File type is enum, all file upload same file type</param>
        /// <param name="domainId">DomainId is unieuqId of domain portal in common values, ex: UEJTHY87UJMK </param>
        /// <param name="folderId">FolderId is uniqueId of restaurant, yacht... , ex: HTYJIU57UYJN</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Files")]
        public async Task<IActionResult> UploadMultiFile(IEnumerable<IFormFile> files, FileTypeEnum fileTypeFid, string domainId, string folderId)
        {
            try
            {
                object result = null;
                foreach (var file in files)
                {
                    result = await _fileService.UploadFile(file, fileTypeFid, domainId, folderId);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Upload file by file byte
        /// </summary>
        /// <param name="model">FolderId is uniqueId of restaurant, yacht... , ex: HTYJIU57UYJN</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FileData")]
        public async Task<IActionResult> UploadFileData(FileDataModel model)
        {
            try
            {
                var result = await _fileService.UploadFileData(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Upload multi file by file data
        /// </summary>
        /// <param name="model">FolderId is uniqueId of restaurant, yacht... , ex: HTYJIU57UYJN</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FileDatas")]
        public async Task<IActionResult> UploadMultiFileData(IEnumerable<FileDataModel> model)
        {
            try
            {
                object result = null;
                foreach (var item in model)
                {
                    result = await _fileService.UploadFileData(item);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Inject file id api will return the file url(string)
        /// </summary>
        /// <param name="id">file id</param>
        /// <returns>String Url</returns>
        [HttpGet]
        [Route("File/{id}")]
        public async Task<IActionResult> GetFileUrl(int id)
        {
            try
            {
                var result = await _fileService.GetFileById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Inject file id and ratio to get file url(string)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("File/{id}/{ratio}")]
        public async Task<IActionResult> GetFileUrl(int id, ThumbRatioEnum ratio)
        {
            try
            {
                var result = await _fileService.GetFileById(id, ratio);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Inject file id to delete the file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("File/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var result = await _fileService.DeleteFileById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Restore all file by secret key
        /// Warning: set includeFileDeleted = true will restore files has been deleted
        /// </summary>
        /// <param name="key"></param>
        /// <param name="includeFileDeleted"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("File/RestoreFile")]
        public async Task<IActionResult> RestoreFile(string key, bool includeFileDeleted = false)
        {
            try
            {
                var result = new RestoreFileResponse();
                if (key == restoreKey)
                {
                    result = await _fileService.RestoreFile(includeFileDeleted);
                    return Ok(result);
                }
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Restore file (type = image)
        /// Inject fileId and ratio to restore image
        /// Warning: set includeFileDeleted = true will restore files has been deleted
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="ratio"></param>
        /// <param name="includeFileDeleted"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("File/RestoreFile/{fileId}/Ratio/{ration}")]
        public async Task<IActionResult> RestoreFile(int fileId, ThumbRatioEnum ratio, bool includeFileDeleted = false)
        {
            try
            {
                var result = await _fileService.RestoreFile(fileId, ratio, includeFileDeleted);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Restore file by fileId
        /// Warning: set includeFileDeleted = true will restore files has been deleted
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="includeFileDeleted"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("File/RestoreFile/{fileId}/{includeFileDeleted}")]
        public async Task<IActionResult> RestoreFile(int fileId, bool includeFileDeleted = false)
        {
            try
            {
                var result = await _fileService.RestoreFile(fileId, includeFileDeleted);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get file info by id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("File/FileInfo/{fileId}")]
        public IActionResult GetFileInfo(int fileId)
        {
            try
            {
                var result = _fileService.GetFileInfo(fileId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("Files/Brochures/{fileId}")]
        public async Task<IActionResult> GetFileBrochure(int fileId)
        {
            var result = await _fileService.GetFileBrochureById(fileId);
            return Ok(result);
        }
        #endregion
    }
}