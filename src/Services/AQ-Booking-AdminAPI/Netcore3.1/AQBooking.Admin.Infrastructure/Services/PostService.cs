using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
using System.Data.Common;
using AQBooking.Admin.Core.Enums;
using System.Security.Cryptography.X509Certificates;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PostService : ServiceBase, IPostService
    {
        #region Fields
        private readonly IPostFileStreamService _postFileStreamService;
        #endregion
        #region Ctor

        public PostService(AQCMSContext dbCMSContext, IWorkContext workContext, IMapper mapper, 
            IPostFileStreamService postFileStreamService) 
            : base(dbCMSContext, workContext, mapper)
        {
            _postFileStreamService = postFileStreamService;
        }
        #endregion

        #region Methods

        #region Post
      
        public PostCreateModel GetPostById(long id)
        {
            var post = _dbCMSContext.Posts.FirstOrDefault(x => x.Id == id && !x.Deleted);
            return _mapper.Map<PostCreateModel>(post);
        }
        public IPagedList<PostViewModel> SearchPost(PostSearchModel searchModel)
        {
            try
            {
                var parameter = new DbParameter[]
                {
                _dbCMSContext.GetParameter("@pPostCategoryFID",searchModel.CategoryFID),
                _dbCMSContext.GetParameter("@pLanguageFID",searchModel.LanguageFID),
                _dbCMSContext.GetParameter("@pIsActivated",searchModel.IsActived),
                _dbCMSContext.GetParameter("@pCreatedBy",searchModel.CreatedBy),
                _dbCMSContext.GetParameter("@pTitle",searchModel.Title),
                _dbCMSContext.GetParameter("@pDeleted",searchModel.IsDeleted),
                _dbCMSContext.GetParameter("@pPageSize",searchModel.PageSize),
                _dbCMSContext.GetParameter("@pPageNumber",searchModel.PageIndex),

                };
                var result = _dbCMSContext.EntityFromSql<PostViewModel>("usp_Post_Search", parameter).ToList();
                int totalRows = 0;
                if (result.Count > 0)
                    totalRows = result[0].TotalRows;
                var pagedListModel = new PagedList<PostViewModel>(result, searchModel.PageIndex, searchModel.PageSize, totalRows);
                return pagedListModel;
            }
            catch (AQException ex)
            {
                throw ex;
            }
        }
        public PostNavigationDetailViewModel GetPostNavigationDetail(PostSearchModel searchModel)
        {
            try
            {
                var parameter = new DbParameter[]
                {
                    _dbCMSContext.GetParameter("@pPostCategoryFID",searchModel.CategoryFID),
                    _dbCMSContext.GetParameter("@pLanguageFID",searchModel.LanguageFID),
                    _dbCMSContext.GetParameter("@pIsActivated",searchModel.IsActived),
                    _dbCMSContext.GetParameter("@pCreatedBy",searchModel.CreatedBy),
                    _dbCMSContext.GetParameter("@pCurrentPostID",searchModel.CurrentPostId),
                };
                var result = _dbCMSContext.EntityFromSql<NavigationInfo>("usp_Post_Next_Prev", parameter).ToList();
                var model = new PostNavigationDetailViewModel();
                if (result.Count > 0)
                {
                    model.PreviousPost = result.FirstOrDefault(item => item.Type == "PREV");
                    model.NextPost = result.FirstOrDefault(item => item.Type == "NEXT");
                }
                return model;
            }
            catch (AQException ex)
            {
                throw ex;
            }
        }
        public long CreateNewPost(PostCreateModel model)
        {
            try
            {

                var entity = _mapper.Map<Posts>(model);
                entity.Deleted = false;
                entity.CreatedBy = _workContext.UserGuid;
                entity.CreatedDate = DateTime.Now;
                entity.UniqueId = UniqueIDHelper.GenerateRandomString(UNIQUE_ID_LENGTH);
                var res = _dbCMSContext.Posts.Add(entity);
                _dbCMSContext.SaveChanges();               
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }
        }
        public long UpdatePost(PostCreateModel model)
        {
            try
            {
                var entity = _dbCMSContext.Posts.Find(model.Id);
                if (entity == null||entity.Deleted)
                    throw new ArgumentNullException("Post");
                entity = _mapper.Map(model, entity);
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                entity.UniqueId = entity.UniqueId?? UniqueIDHelper.GenerateRandomString(UNIQUE_ID_LENGTH);
                _dbCMSContext.SaveChanges();              
              
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }
        }
        public bool DeletePost(int postID)
        {
            try
            {
                var obj = _dbCMSContext.Posts.AsNoTracking().FirstOrDefault(x => x.Id == postID);
                obj.Deleted = true;
                _dbCMSContext.Posts.Update(obj);
                _dbCMSContext.SaveChanges();
                var objDetail = _dbCMSContext.PostDetails.AsNoTracking().Where(x => x.PostFid == postID).ToList();
                objDetail.ForEach(x => x.Deleted = true);
                _dbCMSContext.PostDetails.UpdateRange(objDetail);
                _dbCMSContext.SaveChanges();
                var objFileStream = _dbCMSContext.PostFileStreams.AsNoTracking().Where(x => x.PostFid == postID).ToList();
                _dbCMSContext.PostFileStreams.UpdateRange(objFileStream);
                objFileStream.ForEach(x => x.Deleted = true);
                int res = _dbCMSContext.SaveChanges();
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                throw ex;
            }
        }
        public bool RestorePost(int postID)
        {
            try
            {
                var obj = _dbCMSContext.Posts.AsNoTracking().FirstOrDefault(x => x.Id == postID);
                obj.Deleted = false;
                _dbCMSContext.Posts.Update(obj);
                _dbCMSContext.SaveChanges();
                var objDetail = _dbCMSContext.PostDetails.AsNoTracking().Where(x => x.PostFid == postID).ToList();
                objDetail.ForEach(x => x.Deleted = false);
                _dbCMSContext.PostDetails.UpdateRange(objDetail);
                _dbCMSContext.SaveChanges();
                var objFileStream = _dbCMSContext.PostFileStreams.AsNoTracking().Where(x => x.PostFid == postID).ToList();
                _dbCMSContext.PostFileStreams.UpdateRange(objFileStream);
                objFileStream.ForEach(x => x.Deleted = false);
                int res = _dbCMSContext.SaveChanges();
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                throw ex;
            }
        }
        public bool ChangeStatusPost(int postID,bool isActive)
        {
            try
            {
                var obj = _dbCMSContext.Posts.AsNoTracking().FirstOrDefault(x => x.Id == postID);
                obj.IsActivated = isActive;
                obj.LastModifiedBy = _workContext.UserGuid;
                obj.LastModifiedDate = DateTime.Now;
                _dbCMSContext.Posts.Update(obj);              
                var objDetail = _dbCMSContext.PostDetails.AsNoTracking().Where(x => x.PostFid == postID).ToList();
                objDetail.ForEach(x => {
                    x.IsActivated = isActive;
                    if (isActive)
                    {
                        x.ActivatedBy = _workContext.UserGuid;
                        x.ActivatedDate = DateTime.Now;
                    }
                    else
                    {
                        x.ActivatedBy = null;
                        x.ActivatedDate =null;
                    }
                });             
                int res = _dbCMSContext.SaveChanges();
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                throw ex;
            }
        }
        #endregion



        #endregion
    }
}
