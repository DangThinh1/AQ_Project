using APIHelpers.Response;
using AQBooking.Admin.Core.Enums;
using AQS.BookingAdmin.Infrastructure.Constants;
using AQS.BookingAdmin.Models.Common.FileStream;
using AQS.BookingAdmin.Services.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Controllers
{
    public class FileStreamController:BaseController
    {

        #region Fields
        private readonly IFileStreamService _fileStreamService;
      
        #endregion

        #region Ctor
        public FileStreamController(IFileStreamService fileStreamService)
        {
            _fileStreamService = fileStreamService;
        }
        #endregion

        #region Methods
        #region Actions
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            byte[] CoverImageBytes = null;
            string s = string.Empty;
            BinaryReader reader = new BinaryReader(file.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)file.Length);
            s = Convert.ToBase64String(CoverImageBytes);
            FileDataUploadRequestModel fileUpdateModel = new FileDataUploadRequestModel();
            fileUpdateModel.FileTypeFid = (int)FileTypeEnum.Image;
            fileUpdateModel.FolderId = CommonConstant.POST_IMAGE_FOLDER;
            fileUpdateModel.DomainId = CommonConstant.FILE_STREAM_CMS_DOMAIN;
            fileUpdateModel.FileName = file.FileName.Split('.')[0];
            fileUpdateModel.FileExtention = '.' + file.FileName.Split('.')[1];
            fileUpdateModel.FileData = s;

            var res =await _fileStreamService.UploadFileData(fileUpdateModel);
            if (res != null)
            {
                var fileId = res.ResponseData.FileId;
                var url = await _fileStreamService.GetFileById(fileId, ThumbRatioEnum.full);
                var result = new Tuple<int, string>(fileId, url);
                return Ok(result);
            }

            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UploadTempImage(IFormFile file)
        {
            var (fileName, fileUrl) =await _fileStreamService.UploadTempFile(file, CommonConstant.FILE_TEMP_FOLDER_POST);
            if(fileName==null)
                return BadRequest();
            return Ok(new
            {
                fileName,
                fileUrl
            });
        }
        #endregion
        #region Utilities
        
        #endregion
        #endregion

    }
}
