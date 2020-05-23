using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PostCategories;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PostCategoryService : ServiceBase, IPostCategoryService
    {

        #region Fields

        #endregion

        #region Ctor
        public PostCategoryService(
            AQCMSContext aQCMSContext,
            IWorkContext workContext, IMapper mapper)
            : base(aQCMSContext, workContext, mapper)
        {

        }
        #endregion

        #region Methods

        #region Post category
        public PostCategoriesViewModel GetPostCategoriesById(int id)
        {
            return _mapper.Map<PostCategoriesViewModel>(_dbCMSContext.PostCategories.FirstOrDefault(x => x.Id == id && !x.Deleted));
        }

        public IPagedList<PostCategoriesViewModel> GetPostCategories(PostCategoriesSearchModel searchModel)
        {
            try
            {                
                int totalRows = 0;
                var query = _dbCMSContext.PostCategories
                                         .AsNoTracking()
                                         .Where(x => !x.Deleted)
                                         .OrderBy(x => x.OrderBy);
                var res = _mapper.ProjectTo<PostCategoriesViewModel>(query).ToList();
                if (query.Count() > 0)
                    totalRows = query.Count();
                return new PagedList<PostCategoriesViewModel>(res, searchModel.PageIndex, searchModel.PageSize, totalRows);
            }
            catch (AQException ex)
            {
                throw ex;
            }
        }

        public List<PostCategoriesViewModel> GetParentLst()
        {
            var query = _dbCMSContext.PostCategories
                                     .AsNoTracking()
                                     .Where(x => !x.Deleted
                                        && x.IsActivated)
                                     .OrderBy(x => x.OrderBy);
            return _mapper.ProjectTo<PostCategoriesViewModel>(query).ToList();
        }
        public int CreatePostCategories(PostCategoriesCreateModel model)
        {
            try
            {
                var entity = _mapper.Map<PostCategories>(model);
                if (entity == null)
                    throw new ArgumentNullException("PostCategory");
                entity.CreatedBy = _workContext.UserGuid;
                entity.CreatedDate = DateTime.Now;
                entity.Deleted = false;

                _dbCMSContext.Add(entity);
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
        public int UpdatePostCategories(PostCategoriesCreateModel model)
        {
            try
            {
                var entity = _dbCMSContext.PostCategories.Find(model.Id);
                if (entity == null || entity.Deleted)
                    throw new ArgumentNullException("PostCategory");
                _mapper.Map(model, entity);
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }
        }
        public bool DeletePostCategories(int id)
        {

            try
            {
                var entity = _dbCMSContext.PostCategories.Find(id);
                if (entity == null || entity.Deleted)
                    throw new ArgumentNullException("PostCategory");
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbCMSContext.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return false;
            }
        }


        #endregion

        #region Post category detail
        public List<PostCategoryDetailViewModel> GetPostCateDetailByPostCateId(int postId)
        => _mapper.ProjectTo<PostCategoryDetailViewModel>(
                  _dbCMSContext.PostCategoryDetails
                  .AsNoTracking()
                  .Where(x => x.PostCategoryFid == postId
                  && !x.Deleted))
                  .ToList();

        public PostCategoryDetailViewModel GetPostCategoryDetailByCategoryIdAndLanguageId(int categoryId, int languageId)
        {
            var entity = _dbCMSContext.PostCategoryDetails
                .Where(x =>
                x.PostCategoryFid == categoryId
                && x.LanguageFid == languageId
                && !x.Deleted
                )
                .FirstOrDefault();
            if (entity == null)
                return new PostCategoryDetailViewModel();
            return _mapper.Map<PostCategoryDetailViewModel>(entity);
        }
        public int CreatePostCategoryDetail(PostCategoryDetailCreateModel model)
        {
            try
            {
                var entity = _mapper.Map<PostCategoryDetails>(model);
                if (entity == null)
                    throw new ArgumentNullException("PostCategoryDetail");
                entity.CreatedBy = _workContext.UserGuid;
                entity.CreatedDate = DateTime.Now;
                entity.Deleted = false;
                _dbCMSContext.Add(entity);
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }


        }
        public int UpdatePostCategoryDetail(PostCategoryDetailCreateModel model)
        {
            try
            {
                var entity = _dbCMSContext.PostCategoryDetails.FirstOrDefault(x => x.PostCategoryFid == model.PostCategoryFid && x.Id == model.Id);
                if (entity == null)
                    throw new ArgumentNullException("PostCategoryDetail");
                _mapper.Map(model, entity);
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
        public bool DeletePostCategoryDetail(int id)
        {
            try
            {
                var entity = _dbCMSContext.PostCategoryDetails.FirstOrDefault(x =>
                x.PostCategoryFid == id
                && !x.Deleted);
                if (entity == null)
                    throw new ArgumentNullException("PostCategoryDetail");
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbCMSContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return false;
            }
        }

        public bool CheckPostCategoryDetailDuplicate(PostCategoryDetailCreateModel model)
        {
            bool flagCheck = _dbCMSContext.PostCategoryDetails.Any(x => x.PostCategoryFid == model.PostCategoryFid
                                                                        && x.LanguageFid == model.LanguageFid
                                                                        && x.Id == model.Id);

            return flagCheck;

        }
        #endregion
        #endregion
    }
}
