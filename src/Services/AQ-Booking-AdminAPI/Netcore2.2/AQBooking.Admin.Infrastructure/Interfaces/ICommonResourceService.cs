using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonResource;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface ICommonResourceService
    {
        ApiActionResult Create(CommonResourcesCreateModel model);
        List<CommonResources> GetAll();
        ApiActionResult Update(CommonResourcesUpdateModel model);
        ApiActionResult Delete(CommonResourcesCreateModel model);
        CommonResources GetById(string resKey, int langId);
        List<CommonResourcesKeyValueModel> GetByLangId(int Id, List<string> type = null);
        ApiActionResult CreateNewList(List<CommonResourcesCreateModel> model);
        IPagedList<CommonResourcesViewModel> GetAllCommonResourcesPaging(CommonResourcesSearchModel model);
        List<CommonResources> GetLstResource(int languageID, List<string> type = null);
        List<CommonResources> GetLstByLangId(int LangId);
    }
}
