using APIHelpers;
using APIHelpers.Request;
using AQBooking.Admin.Core.Models.PostCategories;
using AQS.BookingAdmin.Infrastructure;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Models.Paging;
using AQS.BookingAdmin.Models.Users;
using AQS.BookingAdmin.Services.Interfaces.Common;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using Identity.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Common
{
    public class SelectListService : BaseService, ISelectListService
    {
        #region Field
        private readonly IPostCategoryService _postCategoryService;
        private readonly ICommonLanguageService _commonLanguageService;
        #endregion
        #region Ctor
        public SelectListService(IPostCategoryService postCategoryService, ICommonLanguageService commonLanguageService) : base()
        {
            _postCategoryService = postCategoryService;
            _commonLanguageService = commonLanguageService;
        }
        #endregion

        #region Method
        public List<SelectListItem> GetLstByParentId()
        {
            var res = _postCategoryService.GetParentLst().Result;
            if (res != null)
            {
                var lstSelectList = res
                                .Where(c => c.ParentFid == 0)
                                .Select(c => new PostCategoriesViewModel()
                                {
                                    Id = c.Id,
                                    DefaultName = c.DefaultName,
                                    ParentFid = c.ParentFid,
                                    ChildrenLst = SelectListHierarchy.GetChildren(res, c.Id, c.DefaultName)
                                })
                                .ToList();
                return SelectListHierarchy.HierarchyHandle(lstSelectList);
            }
            return new List<SelectListItem>();
        }

        public List<SelectListItem> PreparingLanguageList()
        {
            var languageResponse = _commonLanguageService.GetListLanguage();
            var languages = languageResponse.GetDataResponse();
            if (languages == null)
                languages = new List<SelectListItem>();
            return languages;
        }

        public List<SelectListItem> PreparingCategoryList()
        {
            var categories = GetLstByParentId();
            categories.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            return categories;
        }
        public async Task<List<SelectListItem>> GetUserSelectList(List<int> listRoleId=null)
        {
            try
            {
                var userSelectList = new List<SelectListItem>();
                var model = new UserSearchModel
                {
                    RoleIds = listRoleId,
                    PageSize = -1
                };
                var req = new BaseRequest<UserSearchModel>(model);
                var url = _apiServer.AQIdentityAdminApi.GetCurrentServer() + _adminPortalApiUrl.IdentityAdminAPI.SearchAccounts;
                var res = await _aPIExcute.PostData<PagedListClientModel<UserViewModel>, UserSearchModel>(url, HttpMethodEnum.POST, null, req, Token);
                if (res.IsSuccessStatusCode)
                {
                    var listUser = res.ResponseData;
                    userSelectList = listUser.Data.Select(x => new SelectListItem(x.GetFullName(), x.UserId)).ToList();
                    return userSelectList;
                }

                return userSelectList;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
