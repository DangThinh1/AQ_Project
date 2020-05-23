using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PostService : ServiceBase, IPostService
    {
        #region Ctor
        public PostService(AQCMSContext dbCMSContext, IWorkContext workContext, IMapper mapper) : base(dbCMSContext, workContext, mapper)
        {

        }
        #endregion

        #region Methods
        public ApiActionResult CreateNewPost(PostCreateModel model)
        {
            try
            {
                model.UniqueId = UniqueIDHelper.GenerateRandomString(12);
                Posts obj = new Posts();
                obj = _mapper.Map<Posts>(model);
                obj.LastModifiedDate = model.LastModifiedDate;
                obj.Deleted = false;
                var res = _dbCMSContext.Posts.Add(obj);
                _dbCMSContext.SaveChanges();
                if (res.Entity.Id > 0)
                {
                    model.postFileStream.ForEach(x => x.PostFid = res.Entity.Id);
                    var objFileStream = model.postFileStream.Select(k => new PostFileStreams
                    {
                        FileCategoryFid = k.FileCategoryFid,
                        FileCategoryResKey = k.FileCategoryResKey,
                        PostFid = res.Entity.Id,
                        FileStreamFid = k.FileStreamFid,
                        FileTypeFid = k.FileTypeFid,
                        FileTypeResKey = k.FileTypeResKey,
                        ActivatedDate = k.ActivatedDate,
                        LastModifiedDate = model.LastModifiedDate.Value,
                        LastModifiedBy = k.LastModifiedBy,
                        Deleted = false
                    });
                    _dbCMSContext.PostFileStreams.AddRange(objFileStream);
                    _dbCMSContext.SaveChanges();
                    PostDetails objDetail = new PostDetails();
                    objDetail = _mapper.Map<PostDetails>(model.postDetail);
                    objDetail.LastModifiedDate = model.postDetail.LastModifiedDate;
                    objDetail.Deleted = false;
                    objDetail.PostFid = res.Entity.Id;
                    var resDetail = _dbCMSContext.PostDetails.Add(objDetail);
                    _dbCMSContext.SaveChanges();
                }
                return ApiActionResult.Success();
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
                throw ex;
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
                throw ex;
            }
        }

        public IPagedList<PostViewModel> SearchPost(PostSearchModel searchModel)
        {
            string sortString = !string.IsNullOrEmpty(searchModel.SortString) ? searchModel.SortString : "DefaultTitle ASC";
            var query = (from p in _dbCMSContext.Posts.AsNoTracking()
                         where (p.DefaultTitle.Contains(searchModel.DefaultTitle) || string.IsNullOrEmpty(searchModel.DefaultTitle))
                               && (p.IsActivated == searchModel.IsActivated || searchModel.IsActivated == null)
                               && ((p.CreatedDate >= searchModel.CreatedDateFrom || searchModel.CreatedDateFrom == null)
                               && (p.CreatedDate <= searchModel.CreatedDateTo || searchModel.CreatedDateTo == null))
                               && p.Deleted == false
                         select _mapper.Map<Posts, PostViewModel>(p)).OrderBy(sortString).ToList();

            query.ForEach(x => x.postDetail = _dbCMSContext.PostDetails.AsNoTracking().Where(y => y.PostFid == x.Id).Select(y => _mapper.Map<PostDetails, PostDetailViewModel>(y)).FirstOrDefault());
            query.ForEach(x => x.postFileStream = _dbCMSContext.PostFileStreams.AsNoTracking().Where(y => y.PostFid == x.Id).Select(y => _mapper.Map<PostFileStreams, PostFileStreamViewModel>(y)).ToList());
            return new PagedList<PostViewModel>(query.AsQueryable(), searchModel.PageIndex, searchModel.PageSize);
        }

        public ApiActionResult UpdatePost(PostCreateModel model)
        {
            try
            {
                var obj = _dbCMSContext.Posts.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
                obj.DefaultTitle = model.DefaultTitle;
                obj.TimeToRead = model.TimeToRead;
                obj.IsActivated = model.IsActivated;
                obj.Deleted = model.Deleted;
                obj.CreatedBy = obj.CreatedBy;
                obj.PostCategoryFid = model.PostCategoryFid;
                obj.PostCategoryResKey = model.PostCategoryResKey;
                _dbCMSContext.Update(obj);
                _dbCMSContext.SaveChanges();

                var objDetail = _dbCMSContext.PostDetails.AsNoTracking().FirstOrDefault(x => x.Id == model.postDetail.Id);
                objDetail.KeyWord = model.postDetail.KeyWord;
                objDetail.IsActivated = model.postDetail.IsActivated;
                objDetail.LanguageFid = model.postDetail.LanguageFid;
                objDetail.MetaDescription = model.postDetail.MetaDescription;
                objDetail.Body = model.postDetail.Body;
                objDetail.Deleted = model.postDetail.Deleted;
                objDetail.Title = model.postDetail.Title;
                objDetail.LastModifiedDate = DateTime.Now;
                objDetail.FileStreamFid = model.postDetail.FileStreamFid;
                objDetail.FileTypeFid = model.postDetail.FileTypeFid;

                var objFileStream = _dbCMSContext.PostFileStreams.AsNoTracking().Where(x => x.PostFid == model.Id).ToList();
                for (int i = 0; i < objFileStream.Count; i++)
                {
                    if (objFileStream[i].Id == model.postFileStream[i].Id)
                    {
                        objFileStream[i].ActivatedDate = model.postFileStream[i].ActivatedDate;
                        objFileStream[i].FileCategoryFid = model.postFileStream[i].FileCategoryFid;
                        objFileStream[i].FileCategoryResKey = model.postFileStream[i].FileCategoryResKey;
                        objFileStream[i].FileStreamFid = model.postFileStream[i].FileStreamFid;
                        objFileStream[i].FileTypeFid = model.postFileStream[i].FileTypeFid;
                        objFileStream[i].FileTypeResKey = model.postFileStream[i].FileTypeResKey;
                        objFileStream[i].LastModifiedDate = DateTime.Now;
                    }
                }
                return ApiActionResult.Success();
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }
        #endregion
    }
}
