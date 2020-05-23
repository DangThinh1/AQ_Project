using AQBooking.FileStreamAPI.Core.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;
using AQBooking.FileStreamAPI.Services.Media;
using AQBooking.FileStreamAPI.Core;
using Microsoft.AspNetCore.Http;
using AQBooking.FileStreamAPI.Core.Domain.Response;
using AQBooking.FileStreamAPI.Core.Enums;
using System.IO;
using AQBooking.FileStreamAPI.Core.Helpers;
using AQBooking.FileStreamAPI.Core.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using AQBooking.FileStreamAPI.Core.Domain.Request;
using Microsoft.Extensions.Configuration;

namespace AQBooking.FileStreamAPI.Services.FileService
{
    public class FileService : IFileService
    {
        #region Fields
        private readonly AQ_FileStreamsContext _dataContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IPictureService _pictureService;
        private readonly IAQFileProvider _aqFileProvider;
        private readonly IWebHelper _webHelper;
        private static string host = string.Empty;
        #endregion

        #region Ctor
        public FileService(
            AQ_FileStreamsContext dataContext,
            IPictureService pictureService,
            IAQFileProvider aqFileProvider,
            IWebHelper webHelper,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuaration)
        {
            this._pictureService = pictureService;
            this._aqFileProvider = aqFileProvider;
            this._hostingEnvironment = hostingEnvironment;
            this._webHelper = webHelper;
            this._dataContext = dataContext;
            host = configuaration.GetValue<string>("Host");
        }
        #endregion

        #region Utilities
        public async Task<FileStreamInfo> GetFileInfoById(int id)
        {
            return await Task.Run(() => _dataContext.FileStreamInfo.Where(x => x.FileId == id && x.Deleted == false).SingleOrDefault());
        }

        public async Task<byte[]> GetFileDataById(int id)
        {
            return await Task.Run(() => _dataContext.FileStreamData.Where(x => x.FileId == id).Select(x => x.FileData).SingleOrDefault());
        }

        public async Task<int> InsertFileInfo(FileStreamInfo fileInfo)
        {
            var res = await Task.Run(() => _dataContext.FileStreamInfo.Add(fileInfo));
            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> InsertFileData(FileStreamData fileData)
        {
            var res = await Task.Run(() => _dataContext.FileStreamData.Add(fileData));
            return await _dataContext.SaveChangesAsync();
        }

        public int RestoreFileInfo(int fileId)
        {
            try
            {
                var fileInfo = _dataContext.FileStreamInfo.Where(x => x.FileId == fileId && x.Deleted == true).SingleOrDefault();
                if (fileInfo != null)
                {
                    fileInfo.Deleted = false;
                    _dataContext.FileStreamInfo.Update(fileInfo);
                    return _dataContext.SaveChanges();
                }

                return 0;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public async Task<object> UploadFile(IFormFile file, FileTypeEnum fileType, string domainId, string folderId)
        {
            try
            {
                var isImageFile = _pictureService.IsImageFile(file);
                byte[] fileBinary;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    fileBinary = ms.ToArray();
                }

                var originalName = file.FileName;
                var fileEx = !string.IsNullOrEmpty(Path.GetExtension(file.FileName).ToLower()) ? Path.GetExtension(file.FileName).ToLower() : "." + _pictureService.GetImageFormat(fileBinary).ToString();
                var uniqueId = UniqueIDHelper.GenarateRandomString(12, false);
                var uniqueCode = Guid.NewGuid();
                var fileName = $"{uniqueCode.ToString().Replace("-", "")}{fileEx}";
                var seoName = _webHelper.GetSeName(file.FileName, true, false);
                var isPortraitImage = false;
                var pathRoot = _aqFileProvider.GetAbsolutePath(@"contents\");
                var fullPath = string.Empty;
                var pathSubRoot = string.Empty;
                var pathThumb12 = string.Empty;
                var pathThumb14 = string.Empty;
                var pathThumb16 = string.Empty;
                var pathThumb18 = string.Empty;
                byte[] fileThumb12;
                byte[] fileThumb14;
                byte[] fileThumb16;
                byte[] fileThumb18;

                if (!_aqFileProvider.DirectoryExists(pathRoot))
                    _aqFileProvider.CreateDirectory(pathRoot);

                if (isImageFile)
                {
                    pathSubRoot = Path.Combine(pathRoot, $@"images\");
                }
                else
                {
                    pathSubRoot = Path.Combine(pathRoot, $@"docs\");
                }

                if (!_aqFileProvider.DirectoryExists(pathSubRoot))
                    _aqFileProvider.CreateDirectory(pathSubRoot);

                domainId = !string.IsNullOrEmpty(domainId.ToLower()) ? domainId.ToLower() : "others";
                var pathDomain = Path.Combine(pathSubRoot, $@"{domainId}\");
                if (!_aqFileProvider.DirectoryExists(pathDomain))
                    _aqFileProvider.CreateDirectory(pathDomain);

                folderId = !string.IsNullOrEmpty(folderId.ToLower()) ? folderId.ToLower() : "others";
                var saveFilePath = Path.Combine(pathDomain, $@"{folderId}\");
                if (!_aqFileProvider.DirectoryExists(saveFilePath))
                    _aqFileProvider.CreateDirectory(saveFilePath);

                fullPath = Path.Combine(saveFilePath, $"{fileName}");

                if (isImageFile)
                {
                    int quality = 100;
                    //in MB
                    double fileSize = Convert.ToDouble(fileBinary.Length.ToString()) / 1024 / 1024;
                    if (fileSize >= 4)
                    {
                        quality = 45;
                    }
                    else if (fileSize >= 2 && fileSize < 4)
                    {
                        quality = 65;
                    }
                    else if (fileSize > 0.5 && fileSize < 2)
                    {
                        quality = 85;
                    }

                    fileBinary = _pictureService.ValidatePicture(fileBinary, 2048, quality);

                    fileThumb12 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.half, quality);
                    fileThumb14 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.quarter, quality);
                    fileThumb16 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneSixth, quality);
                    fileThumb18 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneEighth, quality);

                    pathThumb12 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_12{fileEx}");
                    pathThumb14 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_14{fileEx}");
                    pathThumb16 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_16{fileEx}");
                    pathThumb18 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_18{fileEx}");

                    isPortraitImage = _pictureService.isPortraitImage(fileBinary);

                    if (!_aqFileProvider.FileExists(pathThumb12))
                    {
                        using (Stream f = File.OpenWrite(pathThumb12))
                        {
                            await f.WriteAsync(fileThumb12, 0, fileThumb12.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb14))
                    {
                        using (Stream f = File.OpenWrite(pathThumb14))
                        {
                            await f.WriteAsync(fileThumb14, 0, fileThumb14.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb16))
                    {
                        using (Stream f = File.OpenWrite(pathThumb16))
                        {
                            await f.WriteAsync(fileThumb16, 0, fileThumb16.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb18))
                    {
                        using (Stream f = File.OpenWrite(pathThumb18))
                        {
                            await f.WriteAsync(fileThumb18, 0, fileThumb18.Length);
                        };
                    }
                }

                var virtualPath = fullPath.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var virtuaPathThumb12 = pathThumb12.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var virtuaPathThumb14 = pathThumb14.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var virtuaPathThumb16 = pathThumb16.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var virtuaPathThumb18 = pathThumb18.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");

                if (!_aqFileProvider.DirectoryExists(fullPath))
                {
                    using (Stream f = File.OpenWrite(fullPath))
                    {
                        await f.WriteAsync(fileBinary, 0, fileBinary.Length);
                    };
                }

                var fileInfo = new FileStreamInfo()
                {
                    UniqueId = uniqueId,
                    UniqueCode = uniqueCode,
                    FileTypeFid = Convert.ToInt32(fileType),
                    OriginalName = originalName,
                    FileName = fileName,
                    Seoname = seoName,
                    FileExtentions = fileEx,
                    FileSize = fileBinary.Length,
                    Path = virtualPath,
                    PathThumb12 = virtuaPathThumb12,
                    PathThumb14 = virtuaPathThumb14,
                    PathThumb16 = virtuaPathThumb16,
                    PathThumb18 = virtuaPathThumb18,
                    UploadedDateUtc = DateTime.UtcNow,
                    UploadedBy = "",
                    Deleted = false,
                    IsNew = true,
                    IsPortraitImage = isPortraitImage
                };

                await InsertFileInfo(fileInfo);

                var fileData = new FileStreamData()
                {
                    FileId = fileInfo.FileId,
                    FileData = fileBinary
                };

                await InsertFileData(fileData);

                return new FileUploadResponse()
                {
                    FileName = fileInfo.FileName,
                    FileSize = fileInfo.FileSize,
                    FileId = fileInfo.FileId
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> UploadFileData(FileDataModel model)
        {
            try
            {
                //Check filetype
                var isImageFile = _pictureService.GetImageFormat(model.FileData) != ImageFormatEnum.unknown;
                byte[] fileBinary = model.FileData;

                var originalName = model.FileName;
                var fileEx = !string.IsNullOrEmpty(model.FileExtention) ? model.FileExtention : "." + _pictureService.GetImageFormat(model.FileData).ToString();
                var uniqueId = UniqueIDHelper.GenarateRandomString(12, false);
                var uniqueCode = Guid.NewGuid();
                var fileName = $"{uniqueCode.ToString().Replace("-", "")}{fileEx}";
                var seoName = _webHelper.GetSeName(Path.GetFileNameWithoutExtension(originalName), true, false) + fileEx;
                var isPortraitImage = false;
                var pathRoot = _aqFileProvider.GetAbsolutePath(@"contents\");
                var fullPath = string.Empty;
                var pathSubRoot = string.Empty;
                var pathThumb12 = string.Empty;
                var pathThumb14 = string.Empty;
                var pathThumb16 = string.Empty;
                var pathThumb18 = string.Empty;
                byte[] fileThumb12;
                byte[] fileThumb16;
                byte[] fileThumb14;
                byte[] fileThumb18;

                if (!_aqFileProvider.DirectoryExists(pathRoot))
                    _aqFileProvider.CreateDirectory(pathRoot);

                if (isImageFile)
                {
                    pathSubRoot = Path.Combine(pathRoot, $@"images\");
                }
                else
                {
                    pathSubRoot = Path.Combine(pathRoot, $@"docs\");
                }

                if (!_aqFileProvider.DirectoryExists(pathSubRoot))
                    _aqFileProvider.CreateDirectory(pathSubRoot);

                var domainId = !string.IsNullOrEmpty(model.DomainId.ToLower()) ? model.DomainId.ToLower() : "others";
                var pathDomain = Path.Combine(pathSubRoot, $@"{domainId}\");
                if (!_aqFileProvider.DirectoryExists(pathDomain))
                    _aqFileProvider.CreateDirectory(pathDomain);

                var folderid = !string.IsNullOrEmpty(model.FolderId.ToLower()) ? model.FolderId.ToLower() : "others";
                var saveFilePath = Path.Combine(pathDomain, $@"{folderid}\");
                if (!_aqFileProvider.DirectoryExists(saveFilePath))
                    _aqFileProvider.CreateDirectory(saveFilePath);

                fullPath = Path.Combine(saveFilePath, $"{fileName}");

                if (isImageFile)
                {
                    int quality = 100;
                    //in MB
                    double fileSize = Convert.ToDouble(fileBinary.Length.ToString()) / 1024 / 1024;
                    if (fileSize >= 4)
                    {
                        quality = 45;
                    }
                    else if (fileSize >= 2 && fileSize < 4)
                    {
                        quality = 65;
                    }
                    else if (fileSize > 0.5 && fileSize < 2)
                    {
                        quality = 85;
                    }

                    fileBinary = _pictureService.ValidatePicture(fileBinary, 2048, quality);

                    fileThumb12 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.half, quality);
                    fileThumb14 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.quarter, quality);
                    fileThumb16 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneSixth, quality);
                    fileThumb18 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneEighth, quality);

                    pathThumb12 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_12{fileEx}");
                    pathThumb14 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_14{fileEx}");
                    pathThumb16 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_16{fileEx}");
                    pathThumb18 = Path.Combine(saveFilePath, $"{uniqueCode.ToString().Replace("-", "")}_18{fileEx}");

                    isPortraitImage = _pictureService.isPortraitImage(fileBinary);

                    if (!_aqFileProvider.FileExists(pathThumb12))
                    {
                        using (Stream f = File.OpenWrite(pathThumb12))
                        {
                            await f.WriteAsync(fileThumb12, 0, fileThumb12.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb14))
                    {
                        using (Stream f = File.OpenWrite(pathThumb14))
                        {
                            await f.WriteAsync(fileThumb14, 0, fileThumb14.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb16))
                    {
                        using (Stream f = File.OpenWrite(pathThumb16))
                        {
                            await f.WriteAsync(fileThumb16, 0, fileThumb16.Length);
                        };
                    }

                    if (!_aqFileProvider.FileExists(pathThumb18))
                    {
                        using (Stream f = File.OpenWrite(pathThumb18))
                        {
                            await f.WriteAsync(fileThumb18, 0, fileThumb18.Length);
                        };
                    }
                }

                if (!_aqFileProvider.DirectoryExists(fullPath))
                {
                    using (Stream f = File.OpenWrite(fullPath))
                    {
                        await f.WriteAsync(fileBinary, 0, fileBinary.Length);
                    };
                }

                var relativePath = saveFilePath.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var relativePathThumb12 = pathThumb12.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var relativePathThumb14 = pathThumb14.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var relativePathThumb16 = pathThumb16.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");
                var relativePathThumb18 = pathThumb18.Replace($@"{_hostingEnvironment.WebRootPath}\", "").Replace(@"\", "/");

                var fileInfo = new FileStreamInfo()
                {
                    UniqueId = uniqueId,
                    UniqueCode = uniqueCode,
                    FileTypeFid = Convert.ToInt32(model.FileTypeFid),
                    OriginalName = originalName,
                    FileName = fileName,
                    Seoname = seoName,
                    FileExtentions = fileEx,
                    FileSize = fileBinary.Length,
                    Path = relativePath,
                    PathThumb12 = relativePathThumb12,
                    PathThumb14 = relativePathThumb14,
                    PathThumb16 = relativePathThumb16,
                    PathThumb18 = relativePathThumb18,
                    UploadedDateUtc = DateTime.UtcNow,
                    UploadedBy = "",
                    Deleted = false,
                    IsNew = true,
                    IsPortraitImage = isPortraitImage
                };

                await InsertFileInfo(fileInfo);

                var fileStreamData = new FileStreamData()
                {
                    FileId = fileInfo.FileId,
                    FileData = fileBinary
                };

                await InsertFileData(fileStreamData);

                return new FileUploadResponse()
                {
                    FileName = fileInfo.FileName,
                    FileSize = fileInfo.FileSize,
                    FileId = fileInfo.FileId
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetFileById(int id)
        {
            try
            {
                var fileInfo = await GetFileInfoById(id);
                var isSSL = _webHelper.IsCurrentConnectionSecured();
                //var pathHost = _webHelper.GetStoreLocation(isSSL);
                //var host = MyHttpContext.AppBaseUrl;
                var pathHost = host;
                var linkDefault = "";
                if (fileInfo != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.FileName);
                    var path = $"{pathHost}{fileInfo.Path}{fileName}{fileInfo.FileExtentions}";

                    var pathCheck = $@"{_hostingEnvironment.WebRootPath}\{path.Replace($"{pathHost}", "").Replace("/", @"\")}";
                    var isDocPath = pathCheck.Contains("docs");
                    if (_aqFileProvider.FileExists(pathCheck))
                        return path;
                    else
                    {
                        if (isDocPath)
                        {
                            return "File not found!";
                        }
                        else
                        {
                            if (fileInfo.FileTypeFid == (int)FileTypeEnum.Avatar)
                                linkDefault = $@"{pathHost}{AQMediaDefaults.DefaultImagePath}{AQMediaDefaults.DefaultAvatarFileName}";
                            else
                                linkDefault = $@"{pathHost}{AQMediaDefaults.DefaultImagePath}{AQMediaDefaults.DefaultImageFileName}";
                        }
                    }
                }
                else
                {
                    return "File not found!";
                }
                return linkDefault;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetFileById(int id, ThumbRatioEnum ratio)
        {
            try
            {
                var isSSL = _webHelper.IsCurrentConnectionSecured();
                var fileInfo = await GetFileInfoById(id);
                //var pathHost = _webHelper.GetStoreLocation(isSSL);
                var pathHost = host;
                var result = "";
                var linkDefault = "";

                if (fileInfo != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.FileName);
                    switch ((int)ratio)
                    {
                        case 2:
                            result = $"{pathHost}{fileInfo.Path}{fileName}_12{fileInfo.FileExtentions}";
                            break;
                        case 4:
                            result = $"{pathHost}{fileInfo.Path}{fileName}_14{fileInfo.FileExtentions}";
                            break;
                        case 6:
                            result = $"{pathHost}{fileInfo.Path}{fileName}_16{fileInfo.FileExtentions}";
                            break;
                        case 8:
                            result = $"{pathHost}{fileInfo.Path}{fileName}_18{fileInfo.FileExtentions}";
                            break;
                        default:
                            result = $"{pathHost}{fileInfo.Path}{fileName}{fileInfo.FileExtentions}";
                            break;
                    }

                    var pathCheck = $@"{_hostingEnvironment.WebRootPath}\{result.Replace($"{pathHost}", "").Replace("/", @"\")}";
                    if (!_aqFileProvider.FileExists(pathCheck))
                    {
                        if (fileInfo.FileTypeFid == (int)FileTypeEnum.Avatar)
                            linkDefault = $@"{pathHost}{AQMediaDefaults.DefaultImagePath}{AQMediaDefaults.DefaultAvatarFileName}";
                        else
                            linkDefault = $@"{pathHost}{AQMediaDefaults.DefaultImagePath}{AQMediaDefaults.DefaultImageFileName}";
                        return linkDefault;
                    }
                }
                else
                {
                    linkDefault = $@"{pathHost}{AQMediaDefaults.DefaultImagePath}{AQMediaDefaults.DefaultImageFileName}";
                    return linkDefault;
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteFileById(int id)
        {
            try
            {
                var fileInfo = await GetFileInfoById(id);
                var path = fileInfo.Path;
                path = path.Replace("/", @"\");
                var fullPath = $@"{_hostingEnvironment.WebRootPath}\{path}";
                if (_aqFileProvider.FileExists(fullPath))
                    _aqFileProvider.DeleteFile(fullPath);

                fileInfo.Deleted = true;
                var result = _dataContext.FileStreamInfo.Update(fileInfo);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RestoreFile(int fileId, bool includeFileDeleted)
        {
            try
            {
                var fileInfo = _dataContext.FileStreamInfo.Where(x => x.FileId == fileId).FirstOrDefault();
                if (fileInfo != null)
                {
                    if (includeFileDeleted && fileInfo.Deleted == true)
                        RestoreFileInfo(fileInfo.FileId);

                    var pathRoot = _aqFileProvider.GetAbsolutePath();
                    var fileBinary = await GetFileDataById(fileId);
                    var fileName = fileInfo.FileName;
                    var relativePath = fileInfo.Path.Replace("/", @"\");
                    var saveFilePath = $@"{pathRoot}\{relativePath}";
                    var fullPath = $@"{pathRoot}\{relativePath}{fileName}";
                    var checkDirectoryExists = _aqFileProvider.DirectoryExists(saveFilePath);
                    if (!checkDirectoryExists)
                        _aqFileProvider.CreateDirectory(saveFilePath);

                    var checkFileExist = _aqFileProvider.FileExists(fullPath);
                    if (!checkFileExist)
                    {
                        var isImageFile = _pictureService.GetImageFormat(fileBinary) != ImageFormatEnum.unknown;

                        if (isImageFile)
                        {
                            int quality = 100;
                            //in MB
                            double fileSize = Convert.ToDouble(fileBinary.Length.ToString()) / 1024 / 1024;
                            if (fileSize >= 4)
                            {
                                quality = 45;
                            }
                            else if (fileSize >= 2 && fileSize < 4)
                            {
                                quality = 65;
                            }
                            else if (fileSize > 0.5 && fileSize < 2)
                            {
                                quality = 85;
                            }

                            fileBinary = _pictureService.ValidatePicture(fileBinary, 2048, quality);

                        }

                        if (!_aqFileProvider.DirectoryExists(fullPath))
                        {
                            using (Stream f = File.OpenWrite(fullPath))
                            {
                                await f.WriteAsync(fileBinary, 0, fileBinary.Length);
                            };
                        }

                        return true;
                    }
                    return false;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RestoreFile(int fileId, ThumbRatioEnum ratio, bool includeFileDeleted)
        {
            try
            {
                var fileInfo = _dataContext.FileStreamInfo.Where(x => x.FileId == fileId).FirstOrDefault();

                if (fileInfo != null)
                {
                    //Check deletd
                    if (includeFileDeleted && fileInfo.Deleted == true)
                        RestoreFileInfo(fileInfo.FileId);

                    //Data
                    var fileBinary = await GetFileDataById(fileId);
                    byte[] fileThumb12;
                    byte[] fileThumb16;
                    byte[] fileThumb14;
                    byte[] fileThumb18;

                    //Info
                    var fileName = fileInfo.FileName;
                    var fileNameWithoutEx = Path.GetFileNameWithoutExtension(fileName);
                    var fileEx = fileInfo.FileExtentions;

                    //Path
                    var pathRoot = _aqFileProvider.GetAbsolutePath();
                    var relativePath = fileInfo.Path.Replace("/", @"\");
                    var saveFilePath = $@"{pathRoot}\{relativePath}";
                    var fullPath = $@"{saveFilePath}{fileName}";
                    var pathThumb12 = $@"{saveFilePath}{fileNameWithoutEx}_12{fileEx}";
                    var pathThumb14 = $@"{saveFilePath}{fileNameWithoutEx}_14{fileEx}";
                    var pathThumb16 = $@"{saveFilePath}{fileNameWithoutEx}_16{fileEx}";
                    var pathThumb18 = $@"{saveFilePath}{fileNameWithoutEx}_18{fileEx}";

                    var checkDirectoryExists = _aqFileProvider.DirectoryExists(saveFilePath);
                    if (!checkDirectoryExists)
                        _aqFileProvider.CreateDirectory(saveFilePath);

                    var checkFileExist = _aqFileProvider.FileExists(fullPath);
                    if (!checkFileExist)
                    {
                        var isImageFile = _pictureService.GetImageFormat(fileBinary) != ImageFormatEnum.unknown;

                        if (isImageFile)
                        {
                            int quality = 100;
                            //in MB
                            double fileSize = Convert.ToDouble(fileBinary.Length.ToString()) / 1024 / 1024;
                            if (fileSize >= 4)
                            {
                                quality = 45;
                            }
                            else if (fileSize >= 2 && fileSize < 4)
                            {
                                quality = 65;
                            }
                            else if (fileSize > 0.5 && fileSize < 2)
                            {
                                quality = 85;
                            }

                            fileBinary = _pictureService.ValidatePicture(fileBinary, 2048, quality);

                            //Restore by ratio
                            switch ((int)ratio)
                            {
                                case 2:
                                    fileThumb12 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.half, quality);
                                    using (Stream f = File.OpenWrite(pathThumb12))
                                    {
                                        await f.WriteAsync(fileThumb12, 0, fileThumb12.Length);
                                    };
                                    break;
                                case 4:
                                    fileThumb14 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.quarter, quality);
                                    using (Stream f = File.OpenWrite(pathThumb14))
                                    {
                                        await f.WriteAsync(fileThumb14, 0, fileThumb14.Length);
                                    };
                                    break;
                                case 6:
                                    fileThumb16 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneSixth, quality);
                                    using (Stream f = File.OpenWrite(pathThumb16))
                                    {
                                        await f.WriteAsync(fileThumb16, 0, fileThumb16.Length);
                                    };
                                    break;
                                case 8:
                                    fileThumb18 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneEighth, quality);
                                    using (Stream f = File.OpenWrite(pathThumb18))
                                    {
                                        await f.WriteAsync(fileThumb18, 0, fileThumb18.Length);
                                    };
                                    break;
                                default:
                                    using (Stream f = File.OpenWrite(fullPath))
                                    {
                                        await f.WriteAsync(fileBinary, 0, fileBinary.Length);
                                    };
                                    break;
                            }

                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<RestoreFileResponse> RestoreFile(bool includeFileDeleted)
        {
            try
            {
                //First check include deleted
                if (includeFileDeleted)
                {
                    using (var transaction = _dataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var listFileDeleted = (from i in _dataContext.FileStreamInfo
                                                    join d in _dataContext.FileStreamData on i.FileId equals d.FileId
                                                     where i.Deleted == true
                                                      select i).ToList();
                            foreach (var item in listFileDeleted)
                            {
                                RestoreFileInfo(item.FileId);
                            }

                            transaction.Commit();
                            transaction.Dispose();
                        }
                        catch
                        {
                            transaction.Rollback();
                            transaction.Dispose();
                            throw;
                        }
                    } 
                }

                //Start restore and report
                var res = new RestoreFileResponse();
                var report = new ReportModel();
                var reportLines = new List<ReportLine>();
                var pathFileReport = string.Empty;
                var absolutePath = _aqFileProvider.GetAbsolutePath();
                var baseHost = MyHttpContext.AppBaseUrl;
                
                var totalItems = _dataContext.FileStreamInfo.Where(x => x.Deleted == false).Count();
                var getSize = 50;
                var skipNumber = 0;
                var times = totalItems / getSize;
                var totalItemSuccess = 0;
                var totalItemFail = 0;

                var fileInfoItems = _dataContext.FileStreamInfo.Where(x => x.Deleted == false).Skip(skipNumber).Take(getSize).ToList();

                while (times >= 0)
                {
                    res.TotalItems = totalItems;
                    foreach (var fileInfo in fileInfoItems)
                    {
                        var reportLine = new ReportLine();
                        if (fileInfo != null)
                        {
                            reportLine.ReportTime = DateTime.Now;
                            reportLine.FileId = fileInfo.FileId;
                            reportLine.FileName = fileInfo.FileName;
                            reportLine.FileEx = fileInfo.FileExtentions;
                            reportLine.FileSize = fileInfo.FileSize;
                            reportLine.FileType = (int)fileInfo.FileTypeFid;
                            reportLine.Path = fileInfo.Path;

                            var relativePath = fileInfo.Path.Replace("/", @"\");

                            if (string.IsNullOrEmpty(relativePath))
                            {
                                reportLine.Status = "Fail";
                                reportLine.Message = "Path not found";
                                reportLines.Add(reportLine);
                                totalItemFail++;

                                continue;
                            }

                            if (!relativePath.StartsWith("contents\\") || !reportLine.FileName.Contains("."))
                            {
                                reportLine.Status = "Fail";
                                reportLine.Message = "The path is not correct";
                                reportLines.Add(reportLine);
                                totalItemFail++;

                                continue;
                            }

                            try
                            {
                                //Data
                                var fileBinary = await GetFileDataById(fileInfo.FileId);
                                byte[] fileThumb12;
                                byte[] fileThumb16;
                                byte[] fileThumb14;
                                byte[] fileThumb18;

                                //Info
                                var fileName = fileInfo.FileName;
                                var fileNameWithoutEx = Path.GetFileNameWithoutExtension(fileName);
                                var fileEx = fileInfo.FileExtentions;

                                //Path
                                var pathRoot = _aqFileProvider.GetAbsolutePath();
                                var saveFilePath = $@"{pathRoot}\{relativePath}";
                                var fullPath = $@"{saveFilePath}{fileName}";
                                var pathThumb12 = $@"{saveFilePath}{fileNameWithoutEx}_12{fileEx}";
                                var pathThumb14 = $@"{saveFilePath}{fileNameWithoutEx}_14{fileEx}";
                                var pathThumb16 = $@"{saveFilePath}{fileNameWithoutEx}_16{fileEx}";
                                var pathThumb18 = $@"{saveFilePath}{fileNameWithoutEx}_18{fileEx}";

                                var checkDirectoryExists = _aqFileProvider.DirectoryExists(saveFilePath);
                                if (!checkDirectoryExists)
                                    _aqFileProvider.CreateDirectory(saveFilePath);

                                var checkFileExist = _aqFileProvider.FileExists(fullPath);

                                var isImageFile = _pictureService.GetImageFormat(fileBinary) != ImageFormatEnum.unknown;

                                if (isImageFile)
                                {
                                    int quality = 100;
                                    //in MB
                                    double fileSize = Convert.ToDouble(fileBinary.Length.ToString()) / 1024 / 1024;
                                    if (fileSize >= 4)
                                    {
                                        quality = 45;
                                    }
                                    else if (fileSize >= 2 && fileSize < 4)
                                    {
                                        quality = 65;
                                    }
                                    else if (fileSize > 0.5 && fileSize < 2)
                                    {
                                        quality = 85;
                                    }

                                    fileBinary = _pictureService.ValidatePicture(fileBinary, 2048, quality);

                                    if (!_aqFileProvider.DirectoryExists(pathThumb12))
                                    {
                                        fileThumb12 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.half, quality);
                                        using (Stream f = File.OpenWrite(pathThumb12))
                                        {
                                            await f.WriteAsync(fileThumb12, 0, fileThumb12.Length);
                                        };
                                    }

                                    if (!_aqFileProvider.DirectoryExists(pathThumb14))
                                    {
                                        fileThumb14 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.quarter, quality);
                                        using (Stream f = File.OpenWrite(pathThumb14))
                                        {
                                            await f.WriteAsync(fileThumb14, 0, fileThumb14.Length);
                                        };
                                    }

                                    if (!_aqFileProvider.DirectoryExists(pathThumb16))
                                    {
                                        fileThumb16 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneSixth, quality);
                                        using (Stream f = File.OpenWrite(pathThumb16))
                                        {
                                            await f.WriteAsync(fileThumb16, 0, fileThumb16.Length);
                                        };
                                    }

                                    if (!_aqFileProvider.DirectoryExists(pathThumb18))
                                    {
                                        fileThumb18 = _pictureService.ResizePicture(fileBinary, (int)ThumbRatioEnum.oneEighth, quality);
                                        using (Stream f = File.OpenWrite(pathThumb18))
                                        {
                                            await f.WriteAsync(fileThumb18, 0, fileThumb18.Length);
                                        };
                                    }
                                }

                                if (!_aqFileProvider.DirectoryExists(fullPath))
                                {
                                    using (Stream f = File.OpenWrite(fullPath))
                                    {
                                        await f.WriteAsync(fileBinary, 0, fileBinary.Length);
                                    };
                                }
                                reportLine.Status = "Success";
                                reportLines.Add(reportLine);

                                totalItemSuccess++;
                            }
                            catch
                            {
                                reportLines.Add(reportLine);
                                totalItemFail++;
                                continue;
                            }
                        }
                        else
                        {
                            //File item is null
                            reportLine.Status = "Fail";
                            reportLine.Message = "File not found";
                            reportLines.Add(reportLine);
                            totalItemFail++;
                        }
                    }

                    skipNumber += getSize;
                    var listFileInfoTemp = _dataContext.FileStreamInfo.Where(x => x.Deleted == false).Skip(skipNumber).Take(getSize).ToList();
                    fileInfoItems.Clear();
                    fileInfoItems = listFileInfoTemp;
                    times--;
                }

                res.TotalItems = totalItems;
                res.TotalItemFail = totalItemFail;
                res.TotalItemSuccess = totalItemSuccess;

                report.TotalItems = totalItems;
                report.TotalSuccess = totalItemSuccess;
                report.TotalFail = totalItemFail;
                report.DateReport = DateTime.Now;

                var saveFileReport = absolutePath + $"contents/docs/restorefilereports/";
                if (!_aqFileProvider.DirectoryExists(saveFileReport))
                    _aqFileProvider.CreateDirectory(saveFileReport);
                pathFileReport = absolutePath + $"contents/docs/restorefilereports/report_{report.DateReport.ToString("dd-MM-yyyy")}_{DateTime.Now.Ticks}.txt";
                report.ReportLines = reportLines;
                var runNo = 1;

                FileStream fs = File.Create(pathFileReport);
                fs.Close();
                using (StreamWriter file = new StreamWriter(pathFileReport))
                {
                    await file.WriteLineAsync($"Date: {report.DateReport}");
                    await file.WriteLineAsync($"TotalItems: {report.TotalItems}");
                    await file.WriteLineAsync($"Total success: {report.TotalSuccess}");
                    await file.WriteLineAsync($"Total fail: {report.TotalFail}");
                    await file.WriteLineAsync("");

                    foreach (var line in report.ReportLines)
                    {
                        await file.WriteLineAsync($"{runNo++} | {line.ReportTime} | ID: {line.FileId} | Name: {line.FileName} | Size: {line.FileSize} | Type: {line.FileType} | Path: {line.Path} | Status: {line.Status} | Message: {line.Message}");
                    }
                }

                var pathReport = $"{baseHost}/{pathFileReport.Replace(absolutePath, "").Replace(@"\", "/")}";
                res.PathReport = pathReport;
                return res;
            }
            catch
            {
                throw;
            }
        }

        public FileStreamInfo GetFileInfo(int fileId)
        {
            try
            {
                var res = GetFileInfoById(fileId).Result;
                return res;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
