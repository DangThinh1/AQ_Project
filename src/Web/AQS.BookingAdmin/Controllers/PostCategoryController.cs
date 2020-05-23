using AQBooking.Admin.Core.Models.PostCategories;
using AQS.BookingAdmin.Infrastructure;
using AQS.BookingAdmin.Infrastructure.Constants;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Services.Interfaces.Common;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using ExtendedUtility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;


namespace AQS.BookingAdmin.Controllers
{
    public class PostCategoryController : BaseController
    {

        #region Fields
        private readonly ICommonLanguageService _commonLanguageService;
        private readonly IPostCategoryService _postCategoryService;
        private readonly IPostCategoryDetailService _postCategoryDetailService;
        private readonly ISelectListService _selectListService;
        #endregion

        #region Ctor
        public PostCategoryController(ICommonLanguageService commonLanguageService,
                                      IPostCategoryService postCategoryService,
                                      IPostCategoryDetailService postCategoryDetailService,
                                      ISelectListService selectListService
                                      )
        {
            _commonLanguageService = commonLanguageService;
            _postCategoryService = postCategoryService;
            _postCategoryDetailService = postCategoryDetailService;
            _selectListService = selectListService;
        }
        #endregion

        #region Methods
        #region Actions
        #region List
        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListData(PostCategoriesSearchModel model)
        {
            var res = _postCategoryService.SearchPostCategory(model).Result;
            var allLanguages = _selectListService.PreparingLanguageList();
            foreach (var item in res.Data)
            {
                var languageDetails = _postCategoryDetailService.GetPostCateDetailByPostCateId(item.Id).Result;
                var languages = languageDetails.GetDataResponse();
                item.CustomProperties["Languages"] = languages.Select(x => new SelectListItem
                {
                    Text = allLanguages.Find(l => l.Value == x.LanguageFid.ToString())?.Text,
                    Value = x.LanguageFid.ToString(),
                    Selected = x.IsActivated
                });
            }
            ViewBag.recordTotal = res.Data.Count();
            return PartialView("_List", res.Data);
        }
        #endregion
        #region Create / Update / Delete
        public IActionResult Create(int id, int langFId)
        {
            return PartialView("_CreateUpdateModal", LoadCreateUpdatePostCate(id, langFId));
        }

        private PostCategoriesCreateModel LoadCreateUpdatePostCate(int id, int langFId)
        {
            PostCategoriesCreateModel model = new PostCategoriesCreateModel();
            model.IsActivated = true;
            ViewBag.lstLang = _commonLanguageService.GetListLanguage().ResponseData;
            ViewBag.lstParent = _selectListService.GetLstByParentId();
            if (id != 0)
            {
                var postViewModel = _postCategoryService.GetById(id).Result;
                var postCateDetail = _postCategoryDetailService.GetPostCateDetailByPostCateId(id).Result.GetDataResponse();
                foreach (var item in postCateDetail)
                {
                    if (item.LanguageFid == langFId)
                    {
                        model.PostCateDetailId = item.Id;
                        model.Name = item.Name;
                    }
                }
                model.DefaultName = postViewModel.DefaultName;
                model.LanguageFid = langFId != 0 ? langFId : LanguageConstant.DEFAULT_LANGUAGE_ID;
                model.ParentFid = postViewModel.ParentFid;
                model.OrderBy = postViewModel.OrderBy;
                model.IsActivated = postViewModel.IsActivated;
                model.Id = id;
            }
            return model;
        }

        public IActionResult LoadLangFIdChanged(int id, int langFId)
        {
            PostCategoriesCreateModel model = new PostCategoriesCreateModel();
            var postViewModel = _postCategoryService.GetById(id).Result;
            if (id != 0)
            {
                var postCateDetail = _postCategoryDetailService.GetPostCateDetailByPostCateId(id).Result.GetDataResponse();
                foreach (var item in postCateDetail)
                {
                    if (item.LanguageFid == langFId)
                    {
                        model.PostCateDetailId = item.Id;
                        model.Name = item.Name;
                    }
                }
            }
            string resStr = model.Name + "-" + postViewModel.DefaultName + "-" + model.PostCateDetailId;
            return Json(resStr);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(PostCategoriesCreateModel model)
        {

            var postCate = model.Id != 0 ? await _postCategoryService.UpdatePostCategory(model) : await _postCategoryService.CreatePostCategory(model);
            if (postCate != null)
            {
                PostCategoryDetailCreateModel modelDetail = new PostCategoryDetailCreateModel();
                modelDetail.PostCategoryFid = postCate.ResponseData.ToInt32();
                modelDetail.Name = model.Name;
                modelDetail.LanguageFid = model.LanguageFid;
                modelDetail.Id = model.PostCateDetailId;
                var postCateDetail = model.PostCateDetailId != 0 ? _postCategoryDetailService.UpdatePostCategory(modelDetail).Result : _postCategoryDetailService.CreatePostCategory(modelDetail).Result;
                if (postCateDetail != null)
                    return Json(postCate.ResponseData.ToInt32());
            }
            return Json(0);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var resApi = _postCategoryService.DeletePostCategory(id).Result;
            var resDetailApi = _postCategoryDetailService.DeletePostCategory(id).Result;
            return Json(resApi);
        }

        [HttpGet]
        public IActionResult GetListCategory()
        {
            return Ok(_selectListService.GetLstByParentId());
        }

        [HttpPost]
        public IActionResult IsDuplicatePostCateDetail(PostCategoriesCreateModel model)
        {
            var res = _postCategoryDetailService.CheckPostCateDuplicate(model).Result;
            return Json(res);
        }
        #endregion
        #endregion


        #endregion

    }
}
