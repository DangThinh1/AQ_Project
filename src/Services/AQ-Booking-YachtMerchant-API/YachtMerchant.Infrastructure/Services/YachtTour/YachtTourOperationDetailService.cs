using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using YachtMerchant.Core.Models.YachtTourOperationDetails;
using YachtMerchant.Infrastructure.Interfaces;
using AQEncrypts;
using ExtendedUtility;
using AQBooking.Core.Helpers;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourOperationDetailService : IYachtTourOperationDetailService
    {
        private readonly IMapper _mapper;
        private readonly IYachtService _yachtService;
        private readonly YachtOperatorDbContext _db;
        public YachtTourOperationDetailService(YachtOperatorDbContext db, IMapper mapper, IYachtService yachtService)
        {
            _db = db;
            _mapper = mapper;
            _yachtService = yachtService;
        }

        public BaseResponse<bool> Create(YachtTourOperationDetailCreateModel model)
        {
            try
            {
                var entity = GenerateForCreate(model);
                if(entity == null)
                    return BaseResponse<bool>.BadRequest();
                _db.YachtTourOperationDetails.Add(entity);
                var result = _db.SaveChanges();
                return result > 0 
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Creates(List<YachtTourOperationDetailCreateModel> models)
        {
            try
            {
                var entities = models
                    .Select(k => GenerateForCreate(k))
                    .ToList();
                _db.YachtTourOperationDetails.AddRange(entities);
                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(int yachtId, int tourId, YachtTourOperationDetailUpdateModel model)
        {
            try
            {
                var entity = _db.YachtTourOperationDetails
                    .FirstOrDefault(k => !k.Deleted && k.YachtFid == yachtId && k.TourFid == tourId);
                entity = GenerateForUpdate(entity);
                entity.Deleted = true;
                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(int yachtId, int tourId)
        {
            try
            {
                var entity = _db.YachtTourOperationDetails
                    .FirstOrDefault(k => !k.Deleted && k.YachtFid == yachtId && k.TourFid == tourId);
                entity = GenerateForUpdate(entity);
 
                entity.Deleted = true;
                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> SetActivated(int yachtId, int tourId, bool value)
        {
            try
            {
                var entity = _db.YachtTourOperationDetails
                    .FirstOrDefault(k => !k.Deleted && k.YachtFid == yachtId && k.TourFid == tourId);
                entity = GenerateForUpdate(entity);
                if(entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.IsActive = value;
                var result = _db.SaveChanges();
                return result > 0
                    ? BaseResponse<bool>.Success(true)
                    : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourOperationDetailViewModel>> Search(int tourId, YachtTourOperationDetailSearchModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<PagedList<YachtTourOperationDetailViewModel>>.BadRequest();

                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "EffectiveDate";
                var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "DESC";
                var sortString = $"{sortColumn} {sortType}";
                var query = _db.YachtTourOperationDetails
                  .AsNoTracking()
                  .Where(k => !k.Deleted && k.TourFid == tourId)
                  .Select(k => _mapper.Map<YachtTourOperationDetails, YachtTourOperationDetailViewModel>(k));
                var totalItems = query.Count();
                var listOperationDetails = query
                                           .Skip(pageSize * (pageIndex - 1))
                                           .Take(pageSize)
                                           .OrderBy(sortString)
                                           .ToList()
                                           .Select(k=> LoadYachtName(k))
                                           .ToList();
                var pagedList = new PagedList<YachtTourOperationDetailViewModel>(listOperationDetails, totalItems, pageIndex, pageSize);
                return BaseResponse<PagedList<YachtTourOperationDetailViewModel>>.Success(pagedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtTourOperationDetailViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourOperationDetailViewModel>> FindByTourId(int tourId)
        {
            try
            {
                var entities = _db.YachtTourOperationDetails
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.TourFid == tourId)
                    .Select(k=> _mapper.Map<YachtTourOperationDetails, YachtTourOperationDetailViewModel>(k))
                    .ToList();
                return entities != null
                    ? BaseResponse<List<YachtTourOperationDetailViewModel>>.Success(LoadYachtName(entities))
                    : BaseResponse<List<YachtTourOperationDetailViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourOperationDetailViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourOperationDetailViewModel>> FindByYachtId(int yachtId)
        {
            try
            {
                var entities = _db.YachtTourOperationDetails
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.YachtFid == yachtId)
                    .Select(k => _mapper.Map<YachtTourOperationDetails, YachtTourOperationDetailViewModel>(k))
                    .ToList();
                return entities != null
                    ? BaseResponse<List<YachtTourOperationDetailViewModel>>.Success(LoadYachtName(entities))
                    : BaseResponse<List<YachtTourOperationDetailViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourOperationDetailViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region Private Methods
        private YachtTourOperationDetails GenerateForCreate(YachtTourOperationDetailCreateModel model)
        {
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            var entity = _mapper.Map<YachtTourOperationDetailCreateModel, YachtTourOperationDetails>(model);
            entity.Deleted = false;
            entity.IsActive = true;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            return entity;
        }
        private YachtTourOperationDetails GenerateForUpdate(YachtTourOperationDetails entity)
        {
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            return entity;
        }
        private List<YachtTourOperationDetailViewModel> LoadYachtName(List<YachtTourOperationDetailViewModel> models)
        {
            foreach(var item in models)
            {
                var yachtId = item.YachtFid.Decrypt().ToInt32();
                var yacht = _yachtService.GetYachtBasicProfile(yachtId);
                if(yacht != null)
                {
                    item.YachtName = yacht.ResponseData?.YachtName;
                }
            }
            return models;
        }

        private YachtTourOperationDetailViewModel LoadYachtName(YachtTourOperationDetailViewModel model)
        {
            var yachtId = model.YachtFid.Decrypt().ToInt32();
            var yacht = _yachtService.GetYachtBasicProfile(yachtId);
            if (yacht != null)
            {
                model.YachtName = yacht.ResponseData?.YachtName;
            }
            return model;
        }
        #endregion Private Methods
    }
}
