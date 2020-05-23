using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Services.Interfaces;
using AutoMapper;
using Identity.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Common;
using YachtMerchant.Core.Models.YachtTourFileStream;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourFileStreamService : IYachtTourFileStreamService
    {
        private readonly IMapper _mapper;
        protected readonly YachtOperatorDbContext _db;
        private readonly ICommonValueRequestService _commonValueService;

        public YachtTourFileStreamService(YachtOperatorDbContext db, IMapper mapper, ICommonValueRequestService commonValueService)
        {
            _db = db;
            _mapper = mapper;
            _commonValueService = commonValueService;
        }

        public BaseResponse<bool> Update(YachtTourFileStreamUpdateModel model, long fileID)
        {
            try
            {
                var commonValue = _commonValueService.GetListCommonValueByGroup(CommonValueGroupConstant.YachtTourImageCategory);

                var entity = _db.YachtTourFileStreams
                    .FirstOrDefault(k => !k.Deleted && k.Id == fileID);
                entity = GenerateForUpdate(entity);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.InjectFrom(model);

                if (commonValue.IsSuccessStatusCode)
                {
                    entity.FileCategoryResKey = commonValue.IsSuccessStatusCode ?
                        commonValue.ResponseData.FirstOrDefault(x => x.ValueInt == model.FileCategoryFid).ResourceKey : null;
                }
                else
                {
                    entity.FileCategoryResKey = null;
                }

                var result = _db.SaveChanges();
                if (result > 0)
                {
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest(message: "Save Changes fail");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(long fileID)
        {
            try
            {
                var entity = _db.YachtTourFileStreams
                    .FirstOrDefault(k => !k.Deleted && k.Id == fileID);
                entity.Deleted = true;
                _db.YachtTourFileStreams.Update(entity);
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest(message: "Save Changes fail");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(YachtTourFileStreamCreateModel model, int tourId)
        {
            try
            {
                var entity = GenerateEntityForCreate(model, tourId);
                _db.YachtTourFileStreams.Add(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Create(List<YachtTourFileStreamCreateModel> models, int tourId)
        {
            try
            {
                var entities = models.Select(k => GenerateEntityForCreate(k, tourId));
                _db.YachtTourFileStreams.AddRange(entities);
                var result = _db.SaveChanges();
                return result > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourFileStreamViewModel> GetFileStreamById(long fileID)
        {
            try
            {
                var fileStream = _db.YachtTourFileStreams
                    .AsNoTracking()
                    .FirstOrDefault(k => !k.Deleted && k.Id == fileID);

                if (fileStream != null)
                {
                    var model = _mapper.Map<YachtTourFileStreams, YachtTourFileStreamViewModel>(fileStream);
                    return BaseResponse<YachtTourFileStreamViewModel>.Success(model);
                }

                return BaseResponse<YachtTourFileStreamViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourFileStreamViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourFileStreamViewModel>> GetFileStreamByCategoryId(int yachtTourId, int catId)
        {
            try
            {
                var fileStream = _db.YachtTourFileStreams
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.YachtTourFid == yachtTourId && k.FileCategoryFid == catId);

                if (fileStream != null)
                {
                    var model = _mapper.Map<IQueryable<YachtTourFileStreams>, List<YachtTourFileStreamViewModel>>(fileStream);
                    return BaseResponse<List<YachtTourFileStreamViewModel>>.Success(model);
                }

                return BaseResponse<List<YachtTourFileStreamViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourFileStreamViewModel>> GetFileStreamsByTourId(int tourId, YachtTourFileStreamSearchModel searchModel)
        {
            try
            {
                var result = new PagedList<YachtTourFileStreamViewModel>();
                var query = _db.YachtTourFileStreams
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.YachtTourFid == tourId)
                    .Select(k => _mapper.Map<YachtTourFileStreams, YachtTourFileStreamViewModel>(k));

                if (query.Count() > 0)
                    result = new PagedList<YachtTourFileStreamViewModel>(query, searchModel.PageIndex, searchModel.PageSize);
                if (result != null)
                    return BaseResponse<PagedList<YachtTourFileStreamViewModel>>.Success(result);
                return BaseResponse<PagedList<YachtTourFileStreamViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtTourFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region Private Methods

        private YachtTourFileStreams GenerateEntityForCreate(YachtTourFileStreamCreateModel model, int tourId)
        {
            var userId = UserContextHelper.UserId;
            var entity = new YachtTourFileStreams();
            entity = _mapper.Map<YachtTourFileStreamCreateModel, YachtTourFileStreams>(model, entity);

            entity.YachtTourFid = tourId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = DateTime.Now.Date;
            return entity;
        }

        private YachtTourFileStreams GenerateForUpdate(YachtTourFileStreams entity)
        {
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            return entity;
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var entity = _db.YachtTourFileStreams.FirstOrDefault(x => !x.Deleted && x.Id == id);
                if (entity != null)
                    entity.Deleted = true;
                _db.YachtTourFileStreams.Update(entity);
                var result = _db.SaveChanges();
                return result > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #endregion Private Methods
    }
}