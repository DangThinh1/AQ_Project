using AQBooking.Admin.API.Filters;
using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Extentions;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using AQEncrypts;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Constants;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Infrastructure.Helpers;
using AQS.BookingAdmin.Models.Common.FileStream;
using AQS.BookingAdmin.Models.Posts;
using AQS.BookingAdmin.Services.Interfaces.Common;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using AQS.BookingAdmin.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Controllers
{
    [ValidateModel]
    public class PostController : BaseController
    {
        #region Fields

        private readonly IPostService _postService;
        private readonly ICommonLanguageService _commonLanguageService;
        private readonly IPostCategoryService _postCategoryService;
        private readonly IUserService _userService;
        private readonly ISelectListService _selectListService;
        private readonly IFileStreamService _fileStreamService;
        private readonly CommonSettings _commonSettings;
        private readonly IAQFileProvider _aQFileProvider;


        #endregion

        #region Ctor
        public PostController(IPostService postService,
            ICommonLanguageService commonLanguageService,
            IPostCategoryService postCategoryService,
             IUserService userService,
             ISelectListService selectListService,
             IFileStreamService fileStreamService,
             IOptions<CommonSettings> commonSettingsOptions,
             IAQFileProvider aQFileProvider
            )
        {
            _postService = postService;
            _commonLanguageService = commonLanguageService;
            _postCategoryService = postCategoryService;
            _userService = userService;
            _selectListService = selectListService;
            _fileStreamService = fileStreamService;
            _commonSettings = commonSettingsOptions.Value;
            _aQFileProvider = aQFileProvider;
        }
        #endregion

        #region Methods
        #region Actions
        #region List
        public IActionResult List()
        {
            var model = new PostSearchViewModel();
            PreparingPostSearchModel(model);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ListData(PostSearchViewModel model)
        {
            model.LanguageFID = model.LanguageFID == 0 ? LanguageConstant.DEFAULT_LANGUAGE_ID : model.LanguageFID;
            var posts = await _postService.SearchPost(model);
            await PreparingSearchResult(posts.ResponseData);
            return PartialView("_PostListTable", posts.ResponseData);
        }

        [HttpGet]
        public async Task<IActionResult> ListFileStreamOfPostDetail(int postDetailId)
        {
            var fileStreamids = await _postService.GetFileStreamOfPostDetail(postDetailId);
            var results = new List<Tuple<int, string>>();
            foreach (var file in fileStreamids.ResponseData)
            {
                var url = await _fileStreamService.GetFileById(file.FileStreamFid, ThumbRatioEnum.full);
                var result = new Tuple<int, string>(file.FileStreamFid, url);
                results.Add(result);
            }

            return Json(results);
        }
        #endregion

        #region Create / Update / Delete POST
        public async Task<IActionResult> CreateUpdatePost(int id, int languageId)
        {

            var model = new PostCreateViewModel();
            model.LanguageId = languageId == 0 ? LanguageConstant.DEFAULT_LANGUAGE_ID : languageId;
            if (id > 0)
            {
                var post = await _postService.GetPostById(id);
                if (!post.IsSuccessStatusCode)
                    return NotFound();
                model.PostInfo = post.GetDataResponse();
                var postDetail = await _postService.GetPostDetailByPostIdAndLanguageId(id, model.LanguageId);
                model.PostDetail = postDetail.GetDataResponse();
                model.FriendlyUrl = model.PostDetail.FriendlyUrl;


            }
            model.Languages = _selectListService.PreparingLanguageList();
          
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateViewModel model)
        {
          
            var result = await _postService.CreateNewPost(model.PostInfo);
            if (result.IsSuccessStatusCode)
            {
                model.FriendlyUrl = SEOHelper.GetFriendlyUrl(model.PostInfo.DefaultTitle);
                model.PostDetail.PostFid = Convert.ToInt64(result.ResponseData);
                model.PostDetail.LanguageFid = model.LanguageId;             
                model.PostDetail.FriendlyUrl = model.FriendlyUrl;
                await CheckAndUploadFileStream(model, result.ResponseData);
                result = await _postService.CreateNewPostDetail(model.PostDetail);
                
                return Success(model.PostDetail.PostFid);
            }
            return Error(result.Message);

        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(PostCreateViewModel model)
        {
           
            var result = await _postService.UpdatePost(model.PostInfo);
            if (result.IsSuccessStatusCode)
            {
                model.PostDetail.PostFid = Convert.ToInt64(result.ResponseData);
                model.PostDetail.LanguageFid = model.LanguageId;
                
                model.PostDetail.FriendlyUrl = GetPostDetailFriendlyUrl(model.PostInfo);
                await CheckAndUploadFileStream(model, result.ResponseData);
                if (model.PostDetail.Id == 0)
                {
                    model.PostDetail.FriendlyUrl = model.FriendlyUrl;
                    result = await _postService.CreateNewPostDetail(model.PostDetail);
                }                    
                else
                    result = await _postService.UpdatePostDetail(model.PostDetail);

                return Success(model.PostDetail.PostFid);
            }
            return Error(result.Message);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePost(id);
            if (result.IsSuccessStatusCode)
                return Success(id);
            return Error(result.Message);
        }
        [HttpPut]
        public async Task<IActionResult> RestorePost(int id)
        {
            var result = await _postService.RestorePost(id);
            if (result.IsSuccessStatusCode)
                return Success(id);
            return Error(result.Message);
        }
        [HttpPut]
        public async Task<IActionResult>ChangePostStatus(int id,bool isActive)
        {
            var result = await _postService.ChangePostStatus(id,isActive);
            if (result.IsSuccessStatusCode)
                return Success(id);
            return Error(result.Message);
        }
        #endregion

        #region Post Detail

        public IActionResult Preview(long id,int languageId)
        {
            string encrypId = Terminator.Encrypt(id.ToString());
            string url = $"{_commonSettings.FrontEnd_Domain}{_commonSettings.CMS_PostDetail_Preview}/{encrypId}/{languageId}";
            return Redirect(url);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostDetail(long postId, int languageId)
        {
            var model = new PostCreateViewModel
            {
                LanguageId = languageId,
                PostInfo = new PostCreateModel
                {
                    Id = postId
                }
            };
            var result = await _postService.GetPostDetailByPostIdAndLanguageId(postId, languageId);
            model.PostDetail = result.GetDataResponse() ?? new AQBooking.Admin.Core.Models.PostDetail.PostDetailCreateModel();
            return PartialView("_CreateUpdate_MutipleLanguage", model);
        }

        #endregion

        #region File Stream
        private async Task CheckAndUploadFileStream(PostCreateViewModel model, long postId)
        {
            if (postId == 0)
                return;
            if (!string.IsNullOrEmpty(model.ThumbImgName))
            {
                int fileId = await UploadFileStreamFromTempFolder(model.ThumbImgName,postId);
                model.PostDetail.FileStreamFid = fileId;

            }
            //var listFileIds = new List<int>();
            //foreach(var fileName in model.ListImgDescriptions)
            //{
            //    int fileId = await UploadFileStreamFromTempFolder(fileName, postId);
            //    listFileIds.Add(fileId);
            //}
            //model.PostDetail.FileDescriptionIds = listFileIds;
        }
        private async Task<int> UploadFileStreamFromTempFolder(string fileName, long postId)
        {
            var (fileByte, filePath,orignalFileName) = _fileStreamService.GetTempFile(fileName, CommonConstant.FILE_TEMP_FOLDER_POST);
            if (fileByte == null)
                return 0;
            string base64Str = Convert.ToBase64String(fileByte);
            FileDataUploadRequestModel fileUpdateModel = new FileDataUploadRequestModel();
            fileUpdateModel.FileTypeFid = (int)FileTypeEnum.Image;
            fileUpdateModel.FolderId = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(postId.ToString()));
            fileUpdateModel.DomainId = CommonConstant.FILE_STREAM_CMS_DOMAIN;
            fileUpdateModel.FileName = _aQFileProvider.GetFileNameWithoutExtension(orignalFileName);
            fileUpdateModel.FileExtention = _aQFileProvider.GetFileExtension(orignalFileName);
            fileUpdateModel.FileData = base64Str;
            var res = await _fileStreamService.UploadFileData(fileUpdateModel);
            if (res != null)
            {
               int fileId= res.ResponseData.FileId;
                if (fileId > 0)
                    _fileStreamService.DeleteFileTemp(fileName, CommonConstant.FILE_TEMP_FOLDER_POST);
                return fileId;

            }
            return 0;
        }
        
        #endregion

        #region Utilities
        private void PreparingPostSearchModel(PostSearchViewModel model)
        {
            model.Categories = _selectListService.PreparingCategoryList();
        }

        private async Task PreparingSearchResult(PagedListClient<PostViewModel> model)
        {
            if (model == null || model.TotalItems == 0)
                return;
            var allLanguages = _selectListService.PreparingLanguageList();

            var requestUser = (await _selectListService.GetUserSelectList())              
                .ToDictionary(x => x.Value.ToLower(), x => x.Text);

            foreach (var post in model.Data)
            {
                post.CustomProperties["CreatedByName"] =  requestUser[post.CreatedBy.ToString()];
                var languageDetails = await _postService.GetLanguageIdsByPostId(post.PostID);
                var languages = languageDetails.GetDataResponse();
                post.CustomProperties["Languages"] = languages.Select(x => new SelectListItem
                {
                    Text = allLanguages.Find(l => l.Value == x.LanguageId.ToString())?.Text,
                    Value = x.LanguageId.ToString(),
                    Selected = x.IsActivated
                });

            }
        }
        private string GetPostDetailFriendlyUrl(PostCreateModel model)
        {        
           return SEOHelper.GetFriendlyUrl(model.DefaultTitle);
        }
        #endregion
        #endregion

        #endregion
    }
}
