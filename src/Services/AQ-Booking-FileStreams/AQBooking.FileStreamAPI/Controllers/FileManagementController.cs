using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Infrastructure.Interfaces;
using AQBooking.FileStream.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AQBooking.FileStreamAPI.Core.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AQBooking.FileStreamAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class FileManagementController : ControllerBase
    {
        #region Fields
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileHandleService _fileHandleService;
        #endregion

        #region Ctor
        public FileManagementController(
            IFileManagementService fileManagementService,
            IFileHandleService fileHandleService)
        {
            _fileManagementService = fileManagementService;
            _fileHandleService = fileHandleService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("AllFiles")]
        public IActionResult SearchFile([FromQuery]FileSearchModel model)
        {
            var res = _fileManagementService.SearchFile(model);
            return Ok(res);
        }

        [HttpDelete]
        [Route("AllFiles/{id}")]
        public IActionResult RemoveFile(int id)
        {
            var remoteDirect = _fileHandleService.RemoveFileInDirectory(id);
            if (remoteDirect.IsSucceed)
            {
                var res = _fileManagementService.RemoveFile(id);
                return Ok(res);
            }

            return BadRequest("Internal server errol");
        }

        [HttpGet]
        [Route("ImageFiles")]
        public IActionResult SearchImage([FromQuery]FileSearchModel model)
        {
            var res = _fileManagementService.SearchImage(model);
            return Ok(res);
        }

        [HttpGet]
        [Route("BrochureFiles")]
        public IActionResult SearchBrochure([FromQuery]FileSearchModel model)
        {
            var res = _fileManagementService.SearchBrochure(model);
            return Ok(res);
        }

        [HttpGet]
        [Route("DeletedFiles")]
        public IActionResult SearchFileDeleted([FromQuery]FileSearchModel model)
        {
            var res = _fileManagementService.SearchFileDeleted(model);
            return Ok(res);
        }

        [HttpGet]
        [Route("FileStatistical")]
        public IActionResult FileStatistical()
        {
            var res = _fileManagementService.GetFileStatistical();
            return Ok(res);
        }
        #endregion
    }
}