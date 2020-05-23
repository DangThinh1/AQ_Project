using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonLanguague;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface ICommonLanguagueService
    {
        ApiActionResult Create(CommonLanguaguesCreateModel model);
        List<CommonLanguages> GetAll();
        ApiActionResult Update(CommonLanguaguesUpdateModel model);
        Task<bool> Delete(int Id);
        CommonLanguages GetById(int Id);
        IPagedList<CommonLanguaguesViewModel> GetAllCommonLangsPaging(CommonLanguaguesSearchModel model);
        List<SelectListItem> GetLangLstDDL(string paramUnique);
    }
}
