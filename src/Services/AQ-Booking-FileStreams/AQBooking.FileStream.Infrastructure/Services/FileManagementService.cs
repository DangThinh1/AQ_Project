using AQBooking.FileStream.Core.Models;
using AQBooking.FileStream.Core.Models.Common;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using AQBooking.FileStream.Infrastructure.Entities;
using AQBooking.FileStream.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace AQBooking.FileStream.Infrastructure.Services
{
    public class FileManagementService : IFileManagementService
    {
        #region Fields
        private readonly AQ_FileStreamsContext _dataContext;
        #endregion

        #region Ctor
        public FileManagementService(AQ_FileStreamsContext dataContext)
        {
            _dataContext = dataContext;
        }
        #endregion

        #region Methods
        public PagedList<FileViewModel> SearchFile(FileSearchModel model)
        {
            try
            {
                var sortString = string.IsNullOrEmpty(model.SortString) ? "UploadedDateUTC DESC" : model.SortString;
                var query = (from i in _dataContext.FileStreamInfo
                             where i.Deleted == false
                             && (model.FileId == 0 || i.FileId == model.FileId)
                             && (string.IsNullOrEmpty(model.OriginalName) || i.OriginalName.Contains(model.OriginalName))
                             && (string.IsNullOrEmpty(model.UniqueId) || i.UniqueId.Contains(model.UniqueId))
                             && (string.IsNullOrEmpty(model.FileExtension) || i.FileExtentions.Contains(model.OriginalName))
                             && (string.IsNullOrEmpty(model.UploadedDateUTC) || i.UploadedDateUtc == Convert.ToDateTime(model.UploadedDateUTC))
                             select new FileViewModel
                             {
                                 FileId = i.FileId,
                                 UniqueCode = i.UniqueCode,
                                 UniqueId = i.UniqueId,
                                 FileName = i.FileName,
                                 FileExtentions = i.FileExtentions,
                                 FileSize = i.FileSize,
                                 FileTypeFid = i.FileTypeFid,
                                 OriginalName = i.OriginalName,
                                 Seoname = i.Seoname,
                                 Path = i.Path,
                                 IsNew = i.IsNew,
                                 IsPortraitImage = i.IsPortraitImage,
                                 UploadedDateUtc = i.UploadedDateUtc,
                                 UploadedBy = i.UploadedBy,
                                 Deleted = i.Deleted,
                                 DeletedDate = i.DeletedDate
                             }).OrderBy(sortString).AsQueryable();
                return new PagedList<FileViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public PagedList<FileViewModel> SearchImage(FileSearchModel model)
        {
            try
            {
                var sortString = string.IsNullOrEmpty(model.SortString) ? "UploadedDateUTC DESC" : model.SortString;
                var query = (from i in _dataContext.FileStreamInfo
                             where i.Deleted == false
                             && i.FileTypeFid != 9
                             && (model.FileId == 0 || i.FileId == model.FileId)
                             && (string.IsNullOrEmpty(model.OriginalName) || i.OriginalName.Contains(model.OriginalName))
                             && (string.IsNullOrEmpty(model.UniqueId) || i.UniqueId.Contains(model.UniqueId))
                             && (string.IsNullOrEmpty(model.FileExtension) || i.FileExtentions.Contains(model.FileExtension))
                             && (string.IsNullOrEmpty(model.UploadedDateUTC) || i.UploadedDateUtc == Convert.ToDateTime(model.UploadedDateUTC))
                             select new FileViewModel
                             {
                                 FileId = i.FileId,
                                 UniqueCode = i.UniqueCode,
                                 UniqueId = i.UniqueId,
                                 FileName = i.FileName,
                                 FileExtentions = i.FileExtentions,
                                 FileSize = i.FileSize,
                                 FileTypeFid = i.FileTypeFid,
                                 OriginalName = i.OriginalName,
                                 Seoname = i.Seoname,
                                 Path = i.Path,
                                 IsNew = i.IsNew,
                                 IsPortraitImage = i.IsPortraitImage,
                                 UploadedDateUtc = i.UploadedDateUtc,
                                 UploadedBy = i.UploadedBy,
                                 Deleted = i.Deleted,
                                 DeletedDate = i.DeletedDate
                             }).OrderBy(sortString).AsQueryable();
                return new PagedList<FileViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public PagedList<FileViewModel> SearchBrochure(FileSearchModel model)
        {
            try
            {
                var sortString = string.IsNullOrEmpty(model.SortString) ? "UploadedDateUTC DESC" : model.SortString;
                var query = (from i in _dataContext.FileStreamInfo
                             where i.Deleted == false
                             && i.FileTypeFid == 9
                             && (model.FileId == 0 || i.FileId == model.FileId)
                             && (string.IsNullOrEmpty(model.OriginalName) || i.OriginalName.Contains(model.OriginalName))
                             && (string.IsNullOrEmpty(model.UniqueId) || i.UniqueId.Contains(model.UniqueId))
                             && (string.IsNullOrEmpty(model.FileExtension) || i.FileExtentions.Contains(model.FileExtension))
                             && (string.IsNullOrEmpty(model.UploadedDateUTC) || i.UploadedDateUtc == Convert.ToDateTime(model.UploadedDateUTC))
                             select new FileViewModel
                             {
                                 FileId = i.FileId,
                                 UniqueCode = i.UniqueCode,
                                 UniqueId = i.UniqueId,
                                 FileName = i.FileName,
                                 FileExtentions = i.FileExtentions,
                                 FileSize = i.FileSize,
                                 FileTypeFid = i.FileTypeFid,
                                 OriginalName = i.OriginalName,
                                 Seoname = i.Seoname,
                                 Path = i.Path,
                                 IsNew = i.IsNew,
                                 IsPortraitImage = i.IsPortraitImage,
                                 UploadedDateUtc = i.UploadedDateUtc,
                                 UploadedBy = i.UploadedBy,
                                 Deleted = i.Deleted,
                                 DeletedDate = i.DeletedDate
                             }).OrderBy(sortString).AsQueryable();
                return new PagedList<FileViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public PagedList<FileViewModel> SearchFileDeleted(FileSearchModel model)
        {
            try
            {
                var sortString = string.IsNullOrEmpty(model.SortString) ? "UploadedDateUTC DESC" : model.SortString;
                var query = (from i in _dataContext.FileStreamInfo
                             where i.Deleted == true
                             && (model.FileId == 0 || i.FileId == model.FileId)
                             && (string.IsNullOrEmpty(model.OriginalName) || i.OriginalName.Contains(model.OriginalName))
                             && (string.IsNullOrEmpty(model.UniqueId) || i.UniqueId.Contains(model.UniqueId))
                             && (string.IsNullOrEmpty(model.FileExtension) || i.FileExtentions.Contains(model.FileExtension))
                             && (string.IsNullOrEmpty(model.UploadedDateUTC) || i.UploadedDateUtc == Convert.ToDateTime(model.UploadedDateUTC))
                             select new FileViewModel
                             {
                                 FileId = i.FileId,
                                 UniqueCode = i.UniqueCode,
                                 UniqueId = i.UniqueId,
                                 FileName = i.FileName,
                                 FileExtentions = i.FileExtentions,
                                 FileSize = i.FileSize,
                                 FileTypeFid = i.FileTypeFid,
                                 OriginalName = i.OriginalName,
                                 Seoname = i.Seoname,
                                 Path = i.Path,
                                 IsNew = i.IsNew,
                                 IsPortraitImage = i.IsPortraitImage,
                                 UploadedDateUtc = i.UploadedDateUtc,
                                 UploadedBy = i.UploadedBy,
                                 Deleted = i.Deleted,
                                 DeletedDate = i.DeletedDate
                             }).OrderBy(sortString).AsQueryable();
                return new PagedList<FileViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public List<FileStreamInfo> GetAllFile()
        {
            try
            {
                var result = _dataContext.FileStreamInfo.ToList();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse RemoveFile(int id)
        {
            using (var tran = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var fileInfo = _dataContext.FileStreamInfo.Where(x => x.FileId == id).FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileData = _dataContext.FileStreamData.Where(x => x.FileId == fileInfo.FileId).FirstOrDefault();
                        var deleteFileInfo = _dataContext.FileStreamInfo.Remove(fileInfo);
                        var deleteFileData = _dataContext.FileStreamData.Remove(fileData);
                        _dataContext.SaveChanges();

                        tran.Commit();
                        tran.Dispose();
                        return BasicResponse.Succeed("Remove file successfully");
                    }

                    tran.Dispose();
                    return BasicResponse.Failed("Remove file fail");
                }
                catch
                {
                    tran.Rollback();
                    tran.Dispose();
                    throw;
                }
            }
        }

        public FileStatisticalModel GetFileStatistical()
        {
            try
            {
                return new FileStatisticalModel
                {
                    TotalFiles = _dataContext.FileStreamInfo.Count(),
                    TotalImageFiles = _dataContext.FileStreamInfo.Where(x => x.FileTypeFid != 9 && x.Deleted == false).Count(),
                    TotalBrochureFiles = _dataContext.FileStreamInfo.Where(x => x.FileTypeFid == 9 && x.Deleted == false).Count(),
                    TotalDeletedFiles = _dataContext.FileStreamInfo.Where(x => x.Deleted == true).Count()
                };
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
