using AQBooking.FileStreamAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.FileStreamAPI.Services.ImageManagement
{
    public class ImageManagementService : IImageManagementService
    {
        #region Fields
        private readonly AQ_FileStreamsContext _dataContext;
        #endregion

        #region Ctor
        public ImageManagementService(AQ_FileStreamsContext dataContext)
        {
            _dataContext = dataContext;
        }
        #endregion

        #region Methods
        public List<FileStreamInfo> GetAllImage()
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

        public bool DeleteFile(int id)
        {
            using (var tran = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var fileInfo = _dataContext.FileStreamInfo.Where(x => x.FileId == id).FirstOrDefault();
                    var fileData = _dataContext.FileStreamData.Where(x => x.FileId == fileInfo.FileId).FirstOrDefault();
                    var deleteFileInfo = _dataContext.FileStreamInfo.Remove(fileInfo);
                    var deleteFileData = _dataContext.FileStreamData.Remove(fileData);
                    _dataContext.SaveChanges();

                    tran.Commit();
                    tran.Dispose();
                    return true;

                }
                catch
                {
                    tran.Rollback();
                    tran.Dispose();
                    throw;
                }
            }
        }
        #endregion
    }
}
