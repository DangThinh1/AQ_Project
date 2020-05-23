using APIHelpers.Response;
using AQBooking.Admin.Core.Models.PostFileStream;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PostFileStreamService : ServiceBase, IPostFileStreamService
    {
        #region Ctor
        public PostFileStreamService(AQCMSContext dbCMSContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbCMSContext, workContext, mapper)
        {
        }
        #endregion

        #region Methods

        public PostFileStreamViewModel GetPostFileStreamById(long id)
        {
            var entity = _dbCMSContext.PostFileStreams.Find(id);
            return _mapper.Map<PostFileStreamViewModel>(entity);
        }
        public List<PostFileStreamViewModel> GetPostFileStreamByPostId(int postId)
        {
            var query = _dbCMSContext.PostFileStreams.Where(x => !x.Deleted && x.PostFid == postId);
            return _mapper.ProjectTo<PostFileStreamViewModel>(query).ToList();
        }
        public long CreatePostFileStream(PostFileStreamCreateModel model)
        {
            try
            {
                var entity = _mapper.Map<PostFileStreams>(model);
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.Deleted = false;
                _dbCMSContext.PostFileStreams.Add(entity);
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
       
        public long UpdatePostFileStream(PostFileStreamCreateModel model)
        {
            try
            {
                var entity = _dbCMSContext.PostFileStreams.Find(model.Id);
                if (entity == null||entity.Deleted)
                    throw new ArgumentNullException("Post File Stream");
                entity = _mapper.Map(model, entity);
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = _workContext.UserGuid;
                _dbCMSContext.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                AQException.ThrowException(ex);
                return 0;
            }

        }
        public bool DeletePostFileStream(int id)
        {
            try
            {
                var entity = _dbCMSContext.PostFileStreams.Find(id);
                if (entity == null || entity.Deleted)
                    throw new ArgumentNullException("Post File Stream");
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
    }
}
