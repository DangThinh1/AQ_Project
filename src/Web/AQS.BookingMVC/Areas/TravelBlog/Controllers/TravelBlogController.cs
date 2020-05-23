using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.Subscriber;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Areas.TravelBlog.Controllers;
using AQS.BookingMVC.Areas.TravelBlog.Models;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Infrastructure.EmailSender;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Services.Implements.Common;
using AQS.BookingMVC.Services.Interfaces.Post;
using AQS.BookingMVC.Services.Interfaces.Subscribe;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Identity.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AQS.BookingMVC.Infrastructure.ConfigModel;
using Microsoft.Extensions.Options;
using AQEncrypts;
using AQS.BookingMVC.Services.Interfaces;
using AQS.BookingMVC.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using AQS.BookingMVC.Infrastructure.Mvc.Filters;

namespace AQS.BookingMVC.Areas.TravelBlog
{
    
    public class TravelBlogController : BaseTravelBlogController
    {
        #region Fields
        private readonly IPostService _postService;
        private readonly IFileStreamService _fileStreamService;
        private readonly ISubscribeService _subscribeService;
        private readonly IMultiLanguageService _multiLanguageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SUBCRIBER_REQUEST_COOKIE_NAME = "aq_req_sub";
        private readonly CommonSettings _commonSettings;
        private readonly IEmailSender _emailSender;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public TravelBlogController(IPostService postService,
            IFileStreamService fileStreamService,
            ISubscribeService subscribeService,
            IMultiLanguageService multiLanguageService,
            IEmailSender emailSender,
             IHttpContextAccessor httpContextAccessor,
             IOptions<CommonSettings> commonSettingsOptions,
             IWebHelper webHelper,
             IWorkContext workContext
            )
        {
            _postService = postService;
            _fileStreamService = fileStreamService;
            _subscribeService = subscribeService;
            _multiLanguageService = multiLanguageService;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _commonSettings = commonSettingsOptions.Value;
            _webHelper = webHelper;
            _workContext = workContext;
        }
        #endregion

        #region Action   

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>List of travel Blog</returns>
        /// 
        [Route("{lang:lang}/travel-blog", Name = "TravelBlog")]
        [ServiceFilter(typeof(LanguguageParamsFilter))]
        public async Task<IActionResult> Index()
        {
            var searchModel = CreateSearchCondition();
            var response = await _postService.Search(searchModel);
            if((response.IsSuccessStatusCode == false)
                && (response.ResponseData == null))
            {
                return View(new TravelBlogViewModel());
            }
            var travelBlogs = response.GetDataResponse();
            travelBlogs.Data = await PreparingBlogCustomProperties(travelBlogs.Data);
            var viewModel = new TravelBlogViewModel
            {
                TravelBlogItems = travelBlogs
            };

            return View(viewModel);
        }

        /// <summary>
        /// Detail
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <returns>Travel Blog Detail</returns>
        /// 
        [ServiceFilter(typeof(LanguguageParamsFilter))]
        [Route("{lang:lang}/{categoryFriendlyUrl}/{friendlyUrl}-{postId}", Name ="TravelBlogDetail")]
        public async Task<IActionResult> Detail(string lang, 
            string categoryFriendlyUrl, 
            string friendlyUrl, 
            long postId,
            int lang_id
            )
        {
            var currentLanguageId =_workContext.CurrentLanguageId;
            var postDetail = await _postService.GetPostDetail(postId,currentLanguageId);
            var viewModel = new TravelBlogDetailViewModel();

            if (postDetail.IsSuccessStatusCode
                && (postDetail.ResponseData != null)
                && (postDetail.ResponseData.IsActivated)
                && postDetail.ResponseData.FriendlyUrl==friendlyUrl
                )
            {
                var model = postDetail.ResponseData;
                model.CustomProperties["ImageUrl"] = await _fileStreamService.GetFileById(model.FileStreamFid, AQBooking.YachtPortal.Core.Enum.ThumbRatioEnum.quarter);
                model.CustomProperties["CurrentUrl"] = _webHelper.GetThisPageUrl(false);
                model.CustomProperties["DomainUrl"] = _webHelper.GetHostName();
                viewModel.PostDetail = postDetail.ResponseData;
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(viewModel);
        }

        [Route("TravelBlog/PreView/{id}/{languageId}")]
        public async Task<IActionResult> DetailPreView(string id, string languageId)
        {
            string decryptId = Terminator.Decrypt(id);
            var postDetail = await _postService.GetPostDetail(long.Parse(decryptId), int.Parse(languageId));
            var viewModel = new TravelBlogDetailViewModel();

            if (postDetail.IsSuccessStatusCode
                && (postDetail.ResponseData != null))
            {
                viewModel.PostDetail = postDetail.ResponseData;
            }
            else
            {
                return NotFound();
            }

            return View(viewModel);
        }

        /// <summary>
        /// Get Travel Blog
        /// </summary>
        /// <param name="searchModel">Search Model</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetTravelBlog(PostSearchModel searchModel)
        {
            var response = await _postService.Search(searchModel);
            var pageList = response.GetDataResponse();
            pageList.Data = await PreparingBlogCustomProperties(pageList.Data);

            return Json(pageList);
        }

        /// <summary>
        /// Get Search Condition
        /// </summary>
        /// <returns>Search Condition</returns>
        [HttpGet]
        public JsonResult GetSearchCondition()
        {
            var searchCondition = CreateSearchCondition();
            var format = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return Json(JsonConvert.SerializeObject(searchCondition, format));
        }

        /// <summary>
        /// Get Post Nagivation
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <returns>Post Nagivation</returns>
        [HttpGet]
        public async Task<JsonResult> GetPostNagivation(int postId)
        {
            var searchCondition = CreateSearchCondition();
            searchCondition.CurrentPostId = postId;
            var response = await _postService.GetPostNagivation(searchCondition);
            if (response.IsSuccessStatusCode
                && (response.ResponseData != null))
            {
                var viewModel = response.ResponseData;
                var nextPostImageUrl = (viewModel.NextPost != null) && (viewModel.NextPost.FileStreamFid > 0)
                    ? await _fileStreamService.GetFileById(viewModel.NextPost.FileStreamFid, AQBooking.YachtPortal.Core.Enum.ThumbRatioEnum.quarter)
                    : Url.Content(CommonValueConstant.NO_IMAGE_PATH);
                var previousPostImageUrl = (viewModel.PreviousPost != null) && (viewModel.PreviousPost.FileStreamFid > 0)
                    ? await _fileStreamService.GetFileById(viewModel.PreviousPost.FileStreamFid, AQBooking.YachtPortal.Core.Enum.ThumbRatioEnum.quarter)
                    : Url.Content(CommonValueConstant.NO_IMAGE_PATH);
                var jsonResult = new
                {
                    IsExistNext = (viewModel.NextPost != null),
                    NextPostContent = (viewModel.NextPost != null) ? _multiLanguageService.GetResource("NEXTPOST") : string.Empty,
                    NextPostUrl = (viewModel.NextPost != null)
                      ? Url.RouteUrl("TravelBlogDetail", new
                      {
                          lang = _workContext.CurrentLanguageCode.ToLower(),
                          categoryFriendlyUrl = "travel-blog",
                          friendlyUrl = viewModel.NextPost.FriendlyUrl,
                          postId = viewModel.NextPost.PostID,
                          lang_id = _workContext.CurrentLanguageId
                      })
                     : string.Empty,
                    NextPostImageUrl = nextPostImageUrl,
                    NextPostTitle = (viewModel.NextPost != null)
                        ? viewModel.NextPost.Title
                        : string.Empty,
                    IsExsistPre = (viewModel.PreviousPost != null),
                    PreviousPostContent = (viewModel.PreviousPost != null) ? _multiLanguageService.GetResource("PREVIOUSPOST") : string.Empty,
                    PreviousPostUrl = (viewModel.PreviousPost != null)
                       ?Url.RouteUrl("TravelBlogDetail", new
                       {
                           lang = _workContext.CurrentLanguageCode.ToLower(),
                           categoryFriendlyUrl = "travel-blog",
                           friendlyUrl = viewModel.PreviousPost.FriendlyUrl,
                           postId = viewModel.PreviousPost.PostID,
                           lang_id = _workContext.CurrentLanguageId
                       })

                       : string.Empty,
                    PreviousPostImageUrl = previousPostImageUrl,
                    PreviousPostTitle = (viewModel.PreviousPost != null)
                        ? viewModel.PreviousPost.Title
                        : string.Empty
                };

                return Json(jsonResult);
            }

            return Json(response);
        }

        /// <summary>
        /// Subscribe
        /// </summary>
        /// <param name="createModel">Create model</param>
        /// <returns>Success or not</returns>
        [HttpPost]
        [Route("TravelBlog/Subscribe")]
        public async Task<JsonResult> Subscribe(EmailModel model)
        {
            if (!IsValidSubcribeRequest(model.IsCapchaValid))
                return Json(-1);
            
            if (ModelState.IsValid)
            {
                var createModel = new SubscriberCreateModel();
                createModel.IsActivated = true;
                createModel.SourceUrl = "TravelBlog";
                createModel.Email = model.Email;
                createModel.CreatedDate = DateTime.Now;
                createModel.ModuleName = "CMS";
                var response = await _subscribeService.Subscribe(createModel);
                bool isSuccess = response.GetDataResponse();
                if (isSuccess)
                {
                    // Send mail to customer service
                    await _emailSender.SendEmailAsync("New subscribe", string.Format("Email has been subscribe: {0}", model.Email));
                }
                return Json(isSuccess ? 1 : 0);
              
            }

            return Json(0);
        }
        #endregion

        #region Private method
        /// <summary>
        /// Preparing Blog Custom Properties
        /// </summary>
        /// <param name="listPost">List Post</param>
        /// <returns>List Post with custom property</returns>
        private async Task<List<PostViewModel>> PreparingBlogCustomProperties(List<PostViewModel> listPost)
        {
            var authorName = _multiLanguageService.GetResource("TRAVELBLOG_AUTHOR");
            foreach (var item in listPost)
            {
                item.CustomProperties = new Dictionary<string, object>();
                var url = await _fileStreamService.GetFileById(item.FileStreamFID, AQBooking.YachtPortal.Core.Enum.ThumbRatioEnum.full);
                item.CustomProperties["ImageUrl"] = url;
                item.CustomProperties["DetailUrl"] = Url.RouteUrl("TravelBlogDetail", new {
                    lang=_workContext.CurrentLanguageCode.ToLower(), 
                    categoryFriendlyUrl="travel-blog",
                    friendlyUrl = item.FriendlyUrl,
                    postId=item.PostID,
                    lang_id=_workContext.CurrentLanguageId
                });
                item.CustomProperties["Author"] = authorName;
            }

            return listPost;
        }

        /// <summary>
        /// Create Post Search Condition
        /// </summary>
        /// <returns>Post Search Condition</returns>
        private PostSearchModel CreateSearchCondition()
        {
            var languageCode = _workContext.CurrentLanguageId;            
            return new PostSearchModel
            {
                IsActived = true,
                LanguageFID = languageCode,
                PageIndex = 1,
                PageSize = 9,
                SortColumn = "CreatedDate",
                SortType = "DESC"
            };
        }

        private bool IsValidSubcribeRequest(bool isCaptchaValid)
        {
            if (isCaptchaValid||_commonSettings.LimitRequestPerMin==0)
                return true;
            string cookieValue = _httpContextAccessor.HttpContext.GetCookies(SUBCRIBER_REQUEST_COOKIE_NAME);
            if (cookieValue==null)
            {
                cookieValue = "1";
                _httpContextAccessor.HttpContext.SetCookies(Identity.Core.Enums.TimeType.Minute, SUBCRIBER_REQUEST_COOKIE_NAME, cookieValue, 1);
                return true;
            }
            int requestTime = Convert.ToInt32(cookieValue);
            requestTime++;
            if (requestTime > _commonSettings.LimitRequestPerMin)
                return false;
            _httpContextAccessor.HttpContext.SetCookies(Identity.Core.Enums.TimeType.Minute, SUBCRIBER_REQUEST_COOKIE_NAME, requestTime.ToString(), 1);
            return true;

        }
       
        #endregion
    }
}