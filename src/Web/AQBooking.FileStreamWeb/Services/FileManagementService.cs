using AQBooking.FileStream.Core.Models;
using AQBooking.FileStream.Core.Models.Common;
using AQBooking.FileStream.Core.Models.FileRequest;
using AQBooking.FileStream.Core.Models.FileResponse;
using AQBooking.FileStreamWeb.AppConfig;
using AQBooking.FileStreamWeb.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.FileStreamWeb.Services
{
    public class FileManagementService : ServiceBase, IFileManagementService
    {
        #region Fields
        private readonly IOptions<ApiUrlConfig> _apiUrlConfig;
        #endregion

        #region Ctor
        public FileManagementService(IOptions<ApiUrlConfig> apiUrlConfig) : base()
        {
            _apiUrlConfig = apiUrlConfig;
        }
        #endregion

        #region Methods
        public PagedListClientModel<FileViewModel> SearchFile(FileSearchModel model)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.Index;
                var req = new Dictionary<string, object>();
                req.Add("FileId", model.FileId);
                req.Add("OriginalName", model.OriginalName);
                req.Add("UniqueId", model.UniqueId);
                req.Add("FileExtension", model.FileExtension);
                req.Add("UploadedDateUTC", model.UploadedDateUTC);
                req.Add("PageIndex", model.PageIndex);
                req.Add("PageSize", model.PageSize);
                req.Add("SortType", model.SortType);
                req.Add("SortColumn", model.SortColumn);

                var res = _aPIExcute.GetData<PagedListClientModel<FileViewModel>>(url, req, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;

                return new PagedListClientModel<FileViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public PagedListClientModel<FileViewModel> SearchImage(FileSearchModel model)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.ImageManagement;
                var req = new Dictionary<string, object>();
                req.Add("FileId", model.FileId);
                req.Add("OriginalName", model.OriginalName);
                req.Add("UniqueId", model.UniqueId);
                req.Add("FileExtension", model.FileExtension);
                req.Add("UploadedDateUTC", model.UploadedDateUTC);
                req.Add("PageIndex", model.PageIndex);
                req.Add("PageSize", model.PageSize);
                req.Add("SortType", model.SortType);
                req.Add("SortColumn", model.SortColumn);

                var res = _aPIExcute.GetData<PagedListClientModel<FileViewModel>>(url, req, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;

                return new PagedListClientModel<FileViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public PagedListClientModel<FileViewModel> SearchBrochure(FileSearchModel model)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.BrochureManagement;
                var req = new Dictionary<string, object>();
                req.Add("FileId", model.FileId);
                req.Add("OriginalName", model.OriginalName);
                req.Add("UniqueId", model.UniqueId);
                req.Add("FileExtension", model.FileExtension);
                req.Add("UploadedDateUTC", model.UploadedDateUTC);
                req.Add("PageIndex", model.PageIndex);
                req.Add("PageSize", model.PageSize);
                req.Add("SortType", model.SortType);
                req.Add("SortColumn", model.SortColumn);

                var res = _aPIExcute.GetData<PagedListClientModel<FileViewModel>>(url, req, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;

                return new PagedListClientModel<FileViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public PagedListClientModel<FileViewModel> SearchFileDeleted(FileSearchModel model)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.FileDeleted;
                var req = new Dictionary<string, object>();
                req.Add("FileId", model.FileId);
                req.Add("OriginalName", model.OriginalName);
                req.Add("UniqueId", model.UniqueId);
                req.Add("FileExtension", model.FileExtension);
                req.Add("UploadedDateUTC", model.UploadedDateUTC);
                req.Add("PageIndex", model.PageIndex);
                req.Add("PageSize", model.PageSize);
                req.Add("SortType", model.SortType);
                req.Add("SortColumn", model.SortColumn);

                var res = _aPIExcute.GetData<PagedListClientModel<FileViewModel>>(url, req, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;

                return new PagedListClientModel<FileViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteFile(int id)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileHandleUrl.DeleteFile;
                var res = _aPIExcute.PostData<bool, int>($"{url}/{id}", APIHelpers.HttpMethodEnum.DELETE, null, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;
                return res.ResponseData;
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse RemoveFile(int id)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.Index;
                var res = _aPIExcute.PostData<BasicResponse, int>($"{url}/{id}", APIHelpers.HttpMethodEnum.DELETE, null, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;
                return BasicResponse.Failed("Bad request");
            }
            catch
            {
                throw;
            }
        }

        public bool RestoreFile(int id)
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileHandleUrl.RestoreFile;
                var res = _aPIExcute.PostData<bool, object>($"{url}/{id}/{true}", APIHelpers.HttpMethodEnum.PUT, null, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;

                return false;
            }
            catch
            {
                throw;
            }
        }

        public FileStatisticalModel FileStatistical()
        {
            try
            {
                var url = _baseFileStreamUrl + _apiUrlConfig.Value.FileManagementUrl.FileStatistical;
                var res = _aPIExcute.GetData<FileStatisticalModel>(url, null, _basicToken).Result;
                if (res.IsSuccessStatusCode)
                    return res.ResponseData;
                return new FileStatisticalModel();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
