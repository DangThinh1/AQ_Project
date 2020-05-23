using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonResource;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using Microsoft.EntityFrameworkCore;
using AQBooking.Admin.Core.Paging;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class CommonResourceService : ICommonResourceService
    {
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly AQConfigContext _dbConfigContext;
        public CommonResourceService(
            IWorkContext workContext,
            IMapper mapper,
            AQConfigContext dbConfigContext)
        {
            _workContext = workContext;
            _mapper = mapper;
            _dbConfigContext = dbConfigContext;
        }
        public ApiActionResult Create(CommonResourcesCreateModel model)
        {
            try
            {
                CommonResources obj = new CommonResources();
                obj = _mapper.Map<CommonResources>(model);
                _dbConfigContext.CommonResources.Add(obj);
                var flag = _dbConfigContext.SaveChanges();
                if (flag != 0)
                    return ApiActionResult.Success();
                return ApiActionResult.Failed("Created Failed!!");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }

        public ApiActionResult CreateNewList(List<CommonResourcesCreateModel> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        CommonResources obj = new CommonResources();
                        obj = _mapper.Map<CommonResources>(model[i]);
                        if (_dbConfigContext.CommonResources.Any(x => x.ResourceKey == obj.ResourceKey && x.LanguageFid == obj.LanguageFid))
                        {
                            CommonResources updated = _dbConfigContext.CommonResources.Where(x => x.ResourceKey == obj.ResourceKey && x.LanguageFid == obj.LanguageFid).FirstOrDefault();
                            updated.ResourceValue = obj.ResourceValue;
                            updated.TypeOfResource = obj.TypeOfResource;
                            _dbConfigContext.CommonResources.Update(updated);
                            _dbConfigContext.SaveChanges();
                        }
                        else
                        {
                            _dbConfigContext.CommonResources.Add(obj);
                            _dbConfigContext.SaveChanges();
                        }
                    }
                    return ApiActionResult.Success();
                }
                return ApiActionResult.Failed("Created Failed!!");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }

        }

        public ApiActionResult Delete(CommonResourcesCreateModel model)
        {
            try
            {
                var obj = _dbConfigContext.CommonResources.AsNoTracking().Where(x => x.ResourceKey == model.ResourceKey && x.LanguageFid == model.LanguageFid && x.TypeOfResource == model.TypeOfResource).FirstOrDefault();
                if (obj != null)
                {
                    _dbConfigContext.CommonResources.Remove(obj);
                    _dbConfigContext.SaveChanges();
                    return ApiActionResult.Success();
                }
                else
                    return ApiActionResult.Failed("Can not find data to delete");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }

        }
        public List<CommonResources> GetAll() => _dbConfigContext.CommonResources.AsNoTracking().ToList();


        public IPagedList<CommonResourcesViewModel> GetAllCommonResourcesPaging(CommonResourcesSearchModel model)
        {
            string sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "ResourceKey ASC";
            var result = (from value in _dbConfigContext.CommonResources.AsNoTracking()
                          where (string.IsNullOrEmpty(model.ResourceKey) || value.ResourceKey.Contains(model.ResourceKey))
                          && (string.IsNullOrEmpty(model.ResourceValue) || value.ResourceValue.Contains(model.ResourceValue))
                          && (model.LanguageFid == null || value.LanguageFid == model.LanguageFid)
                          select _mapper.Map<CommonResources, CommonResourcesViewModel>(value)).OrderBy(sortString).AsQueryable();

            return new PagedList<CommonResourcesViewModel>(result, model.PageIndex, model.PageSize);
        }

        public CommonResources GetById(string resKey, int langId) => _dbConfigContext.CommonResources.AsNoTracking().FirstOrDefault(x => x.ResourceKey.Contains(resKey) && x.LanguageFid == langId);


        public List<CommonResourcesKeyValueModel> GetByLangId(int Id, List<string> type = null)
        {
            throw new NotImplementedException();
        }

        public ApiActionResult Update(CommonResourcesUpdateModel model)
        {
            try
            {
                CommonResources obj = new CommonResources();
                obj = _dbConfigContext.CommonResources.AsNoTracking().FirstOrDefault(x => x.ResourceKey == model.ResourceKey);
                if (obj != null)
                {
                    obj.ResourceValue = model.ResourceValue;
                    obj.TypeOfResource = model.TypeOfResource;
                    _dbConfigContext.CommonResources.Update(obj);
                    var flag = _dbConfigContext.SaveChanges();
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

        public List<CommonResources> GetLstResource(int languageID, List<string> type = null)
        {
            List<string> typesearch = type.Count() > 0 ? type : new List<string>() { "COMMON" };
            var res = (from r in _dbConfigContext.CommonResources.AsNoTracking()
                       where (typesearch.Contains(r.TypeOfResource.Trim().ToUpper()))
                       && r.LanguageFid == languageID
                       select r).ToList();
            if (res != null)
                return res;
            return null;
        }

        public List<CommonResources> GetLstByLangId(int LangId) => _dbConfigContext.CommonResources.AsNoTracking().Where(x => x.LanguageFid == LangId).ToList();


    }
}
