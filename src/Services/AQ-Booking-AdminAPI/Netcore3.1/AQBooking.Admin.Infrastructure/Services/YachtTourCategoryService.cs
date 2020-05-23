using AQBooking.Admin.Core.Models.YachtTourCategory;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class YachtTourCategoryService : ServiceBase, IYachtTourCategoryService
    {
        #region Fields
        #endregion

        #region Ctor
        public YachtTourCategoryService(AQYachtContext yachtContext,
            AQConfigContext configContext,
            IWorkContext workContext, 
            IMapper mapper) : base(configContext, yachtContext, mapper)
        {
        }
        #endregion Ctor

        public bool Create(YachtTourCategoryCreateModel model)
        {
            using (var transaction = _dbYachtContext.Database.BeginTransaction())
            {
                try
                {
                    var _userId = _workContext.UserGuid;
                    var _date = DateTime.Now;

                    if (model == null)
                        return false;

                    DateTime? activetdDate = null;
                    if (model.ActivatedDate.HasValue)
                        activetdDate = model.ActivatedDate.Value.Date;

                    var info = new YachtTourCategories();
                    info = _mapper.Map<YachtTourCategoryCreateModel, YachtTourCategories>(model, info);
                    info.IsActivated = true;
                    info.Deleted = false;
                    info.LastModifiedBy = _userId;
                    info.LastModifiedDate = _date;
                    info.CreatedBy = _userId;
                    info.CreatedDate = _date;
                    _dbYachtContext.YachtTourCategories.Add(info);
                    _dbYachtContext.SaveChanges();

                    var detail = new YachtTourCategoryInfomations();
                    detail = _mapper.Map<YachtTourCategoryCreateModel, YachtTourCategoryInfomations>(model, detail);
                    detail.TourCategoryFid = info.Id;
                    detail.Deleted = false;
                    detail.LastModifiedBy = _userId;
                    detail.LastModifiedDate = _date;
                    detail.CreatedBy = _userId;
                    detail.CreatedDate = _date;
                    _dbYachtContext.YachtTourCategoryInfomations.Add(detail);
                    _dbYachtContext.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool Delete(int id)
        {
            try
            {
                DateTime date = DateTime.Now;
                var res = _dbYachtContext.YachtTourCategories.FirstOrDefault(r => r.Deleted == false && r.Id == id);
                if (res == null)
                    return false;

                res.Deleted = true;
                res.LastModifiedBy = GetCurrentUserId();
                res.LastModifiedDate = date;

                var details = _dbYachtContext.YachtTourCategoryInfomations.Where(x => x.TourCategoryFid == id).ToList();
                if (details.Count > 0)
                {
                    foreach (var item in details)
                    {
                        item.Deleted = true;
                        item.LastModifiedBy = GetCurrentUserId();
                        item.LastModifiedDate = date;
                    }
                }
                _dbYachtContext.SaveChanges();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public YachtTourCategoryCreateModel FindInfoDetailById(long id)
        {
            try
            {
                var tourCate = new YachtTourCategoryCreateModel();
                var entity = _dbYachtContext.YachtTourCategoryInfomations.AsNoTracking()
                    .FirstOrDefault(e => e.Deleted == false && e.Id == id);
                if (entity == null)
                    return tourCate;
                var model = new YachtTourCategoryCreateModel();
                
                return tourCate;
            }
            catch
            {
                throw;
            }
        }

        public List<YachtTourCategorySupportModel> GetInfoDetailSupportedByTourCategoryId(int tourCateId)
        {
            throw new NotImplementedException();
        }

        public bool IsActivated(int id, bool value)
        {
            try
            {
                var check = _dbYachtContext.YachtTourCategories.Any(x => x.Id == id && !x.Deleted && x.IsActivated == value);
                if (check)
                    return false;
                var entity = _dbYachtContext.YachtTourCategories.FirstOrDefault(x => x.Id == id && x.Deleted == false);
                if (entity == null)
                    return false;

                entity.IsActivated = value;
                entity.LastModifiedBy = GetCurrentUserId();
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<YachtTourCategoryViewModel> Search(YachtTourCategorySearchModel model)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "ActivatedDate DESC";

                var commonLang = _dbConfigContext.CommonLanguages.Where(x=>!x.Deleted).ToList();
                var query = (from i in _dbYachtContext.YachtTourCategories
                             .Where(k => !k.Deleted)
                             select new YachtTourCategoryViewModel()
                             {
                                 Id = i.Id,
                                 DefaultName = i.DefaultName,
                                 ResourceKey = i.ResourceKey,
                                 IsActivated = i.IsActivated,
                                 ActivatedDate = i.ActivatedDate,
                                 LanguagesSupported = (from d in _dbYachtContext.YachtTourCategoryInfomations
                                                       join l in commonLang on d.LanguageFid equals l.Id
                                                       where !d.Deleted && d.TourCategoryFid == i.Id
                                                       select l.ResourceKey).ToList()
                             }).OrderBy(x=>x.OrderBy);
                var result = new PagedList<YachtTourCategoryViewModel>(query, model.PageIndex, model.PageSize);
                if (result != null)
                    return result;
                return new PagedList<YachtTourCategoryViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public bool Update(YachtTourCategoryCreateModel model)
        {
            try
            {
                if (model == null)
                    return false;

                var entity = _dbYachtContext.YachtTourCategoryInfomations.FirstOrDefault(e => e.Deleted == false && e.Id == model.Id);
                entity = _mapper.Map<YachtTourCategoryCreateModel, YachtTourCategoryInfomations>(model, entity);
                entity.LastModifiedBy = GetCurrentUserId();
                entity.LastModifiedDate = DateTime.UtcNow;
                if (model.ActivatedDate.HasValue)
                {
                    model.ActivatedDate = model.ActivatedDate.Value.Date;
                }
                if (model.FileStreamFid > 0)
                    entity.FileStreamFid = model.FileStreamFid;
                if (model.FileTypeFid > 0)
                    entity.FileTypeFid = model.FileTypeFid;

                _dbYachtContext.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}