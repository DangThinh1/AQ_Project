using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.PostDetail;
using AQBooking.Admin.Core.Models.PostFileStream;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PostDetailService : ServiceBase, IPostDetailService
    {
        #region Fields
        private readonly IPostFileStreamService _postFileStreamService;
        #endregion

        #region Ctor

        public PostDetailService(AQCMSContext dbCMSContext, IWorkContext workContext, IMapper mapper,
            IPostFileStreamService postFileStreamService)
            : base(dbCMSContext, workContext, mapper)
        {
            _postFileStreamService = postFileStreamService;
        }
        #endregion

        #region Methods

        #region Post Details
        public PostDetailViewModel GetPostDetailById(long postDetailId)
        {
            return _mapper.Map<PostDetailViewModel>(_dbCMSContext.PostDetails.Find(postDetailId));
        }
        public PostDetailCreateModel GetPostDetailByPostIdAndLanguageId(long postId, int langugeId)
        {
            var query = _dbCMSContext.PostDetails.FirstOrDefault(x => 
            !x.Deleted
            && x.PostFid == postId
            && x.LanguageFid == langugeId);
            return _mapper.Map<PostDetailCreateModel>(query);
        }
        public PostDetailViewModel GetPostDetailViewByPostIdAndLanguageId(int postId, int langugeId)
        {
            return _mapper.Map<PostDetailViewModel>(_dbCMSContext.PostDetails.FirstOrDefault(x =>
            !x.Deleted&& 
            x.PostFid == postId
            && x.LanguageFid == langugeId
            ));
        }
        public List<PostDetailLanguageViewModel> GetLanguageIdsByPostId(int postId)
        {
            return _dbCMSContext.PostDetails
                .Where(x => !x.Deleted && x.PostFid == postId)
                .Select(x => new PostDetailLanguageViewModel
                {
                    LanguageId = x.LanguageFid,
                    IsActivated = x.IsActivated,
                    PostDetailId = x.Id
                }).ToList()
                ;

        }
        public long CreatePostDetail(PostDetailCreateModel model)
        {
            try
            {
                var existsPost = GetPostDetailByPostIdAndLanguageId(model.PostFid, model.LanguageFid);
                if (existsPost != null)
                    throw new ArgumentNullException($"Post is  existing with language={model.LanguageFid} and postId={model.PostFid}");

                var entity = _mapper.Map<PostDetails>(model);                
                entity.UniqueId = UniqueIDHelper.GenerateRandomString(UNIQUE_ID_LENGTH);
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.Deleted = false;
               
                _dbCMSContext.PostDetails.Add(entity);
                _dbCMSContext.SaveChanges();
                if (entity.Id > 0)
                {
                    if (model.FileStreamFid > 0)
                    {
                        entity.FileTypeFid = (int)FileTypeEnum.Image;
                        var fileStreamModel = PreparingPostFileStreamCreateModel(entity, PostFileCategoryEnum.Thumbnail);
                        _postFileStreamService.CreatePostFileStream(fileStreamModel);

                    }
                    if (model.FileDescriptionIds.Count > 0)
                    {
                        foreach (var id in model.FileDescriptionIds)
                        {
                            var item = PreparingPostFileStreamCreateModel(entity, PostFileCategoryEnum.Detail);
                            item.FileStreamFid = id;
                            _postFileStreamService.CreatePostFileStream(item);
                        }

                    }
                }

                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
        public long UpdatePostDetail(PostDetailCreateModel model)
        {
            try
            {
                var entity = _dbCMSContext.PostDetails.Find(model.Id);
                if (entity == null||entity.Deleted)
                    throw new ArgumentNullException("Post detail");
                int oldFileStreamId = entity.FileStreamFid;
                entity = _mapper.Map(model, entity);
                entity.UniqueId = entity.UniqueId ?? UniqueIDHelper.GenerateRandomString(UNIQUE_ID_LENGTH);
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = _workContext.UserGuid;
                _dbCMSContext.SaveChanges();
                if (entity.Id > 0)
                {
                    if (model.FileStreamFid > 0 && oldFileStreamId != model.FileStreamFid)
                    {

                        #region Insert new post file stream     
                        entity.FileTypeFid = (int)FileTypeEnum.Image;
                        var fileStreamModel = PreparingPostFileStreamCreateModel(entity, PostFileCategoryEnum.Thumbnail);
                        _postFileStreamService.CreatePostFileStream(fileStreamModel);
                        _postFileStreamService.DeletePostFileStream(oldFileStreamId);
                        #endregion
                    }
                    if (model.FileDescriptionIds.Count > 0)
                    {

                        foreach (var id in model.FileDescriptionIds)
                        {
                            var item = PreparingPostFileStreamCreateModel(entity, PostFileCategoryEnum.Detail);
                            item.FileStreamFid = id;
                            _postFileStreamService.CreatePostFileStream(item);
                        }


                    }

                    var havePostDetailActive = _dbCMSContext.PostDetails.Count(x=>x.PostFid==entity.PostFid&& x.IsActivated)>0;

                    var post = _dbCMSContext.Posts.Find(entity.PostFid);
                    post.IsActivated = havePostDetailActive;
                    post.LastModifiedBy = _workContext.UserGuid;
                    post.LastModifiedDate = DateTime.Now;
                    _dbCMSContext.SaveChanges();
                   
                }
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
        public bool DeletePostDetail(int id)
        {
            try
            {
                var entity = _dbCMSContext.PostDetails.Find(id);
                if (entity == null || entity.Deleted)
                    throw new ArgumentNullException("Post detail");
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

        #region Utilities
        private PostFileStreamCreateModel PreparingPostFileStreamCreateModel(PostDetails postDetails, PostFileCategoryEnum postFileCategoryEnum)
        {
            return new PostFileStreamCreateModel
            {
                PostFid = postDetails.PostFid,
                FileCategoryFid = (int)postFileCategoryEnum,
                FileCategoryResKey = "POST_FILE_CATEGORY_" + postFileCategoryEnum.ToString().ToUpper(),
                ActivatedDate = DateTime.Now,
                FileStreamFid = postDetails.FileStreamFid,
                FileTypeFid = (int)FileTypeEnum.Image,
                FileTypeResKey = "FILETYPE_" + FileTypeEnum.Image.ToString().ToUpper()
            };
        }

       
        #endregion

        #endregion

    }
}
