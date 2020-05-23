using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonLanguague;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using Microsoft.EntityFrameworkCore;
using AQBooking.Admin.Core.Paging;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class CommonLanguagueService : ICommonLanguagueService
    {
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly AQConfigContext _dbConfigContext;
        public CommonLanguagueService(
            IWorkContext workContext, 
            IMapper mapper, 
            AQConfigContext aqcontext)
        {
            _workContext = workContext;
            _mapper = mapper;
            _dbConfigContext = aqcontext;
        }

        public ApiActionResult Create(CommonLanguaguesCreateModel model)
        {
            try
            {
                CommonLanguages obj = new CommonLanguages();
                obj = _mapper.Map<CommonLanguages>(model);
                obj.UniqueId = UniqueIDHelper.GenerateRandomString(12);

                _dbConfigContext.CommonLanguages.Add(obj);
                _dbConfigContext.SaveChanges();
                return ApiActionResult.Success();
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }

        public Task<bool> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public List<CommonLanguages> GetAll() => _dbConfigContext.CommonLanguages.AsNoTracking().ToList();


        public IPagedList<CommonLanguaguesViewModel> GetAllCommonLangsPaging(CommonLanguaguesSearchModel model)
        {
            string sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "LanguageName ASC";
            var result = (from lang in _dbConfigContext.CommonLanguages.AsNoTracking()
                          where (lang.LanguageName.Contains(model.LanguageName) || string.IsNullOrEmpty(model.LanguageName))
                          && (string.IsNullOrEmpty(model.LanguageCode) || lang.LanguageCode.Contains(model.LanguageCode))
                          select _mapper.Map<CommonLanguaguesViewModel>(lang)).OrderBy(sortString).AsQueryable();

            return new PagedList<CommonLanguaguesViewModel>(result, model.PageIndex, model.PageSize);
        }

        public CommonLanguages GetById(int Id) => _dbConfigContext.CommonLanguages.AsNoTracking().FirstOrDefault(x => x.Id == Id);

        public ApiActionResult Update(CommonLanguaguesUpdateModel model)
        {
            try
            {
                CommonLanguages obj = new CommonLanguages();
                obj = _dbConfigContext.CommonLanguages.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    obj.LanguageCode = model.LanguageCode;
                    obj.LanguageName = model.LanguageName;
                    obj.Remarks = model.Remarks;
                    obj.ResourceKey = model.ResourceKey;
                    _dbConfigContext.CommonLanguages.Update(obj);
                    int flag = _dbConfigContext.SaveChanges();
                    if (flag != 0)
                        return ApiActionResult.Success();
                    return ApiActionResult.Failed("Updated failed");
                }
                return ApiActionResult.Failed("Can not updated because entity doest not existed!!");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }

        public List<SelectListItem> GetLangLstDDL(string paramUnique)
        {
            if (!string.IsNullOrEmpty(paramUnique))
            {
                var portal = _dbConfigContext.PortalLanguageControls.AsNoTracking().Where(x => x.PortalUniqueId.Contains(paramUnique)).ToList();
                var res = (from l in _dbConfigContext.CommonLanguages.AsNoTracking()
                           where portal.Select(x => x.LanguageFid).Contains(l.Id)
                           select (new SelectListItem
                           {
                               Text = l.LanguageName,
                               Value = l.Id.ToString()
                           })).ToList();

                if (res != null)
                    return res;
            }

            return new List<SelectListItem>();
        }
    }
}
