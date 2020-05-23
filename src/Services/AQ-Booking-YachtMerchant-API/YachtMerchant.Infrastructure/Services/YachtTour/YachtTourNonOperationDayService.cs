using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using YachtMerchant.Core.Models.YachtTourNonOperationDays;
using YachtMerchant.Infrastructure.Interfaces;
using AQEncrypts;
using ExtendedUtility;
using AQBooking.Core.Helpers;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourNonOperationDayService : IYachtTourNonOperationDayService
    {
        private readonly IMapper _mapper;
        private readonly IYachtService _yachtService;
        private readonly YachtOperatorDbContext _db;

        public YachtTourNonOperationDayService(YachtOperatorDbContext db, IMapper mapper, IYachtService yachtsService)
        {
            _db = db;
            _mapper = mapper;
            _yachtService = yachtsService;
        }

        public BaseResponse<bool> Create(List<YachtTourNonOperationDayCreateModel> models)
        {
            try
            {
                var entities = new List<YachtTourNonOperationDays>();
                foreach (var m in models)
                    entities.Add(GenerateCreateModel(m));
                _db.YachtTourNonOperationDays.AddRange(entities);
                var result = _db.SaveChanges();
                if(result > 0)
                {
                    return BaseResponse<bool>.Success(true);
                }

                return BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
               return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourNonOperationDayViewModel>> FindByTourFid(int tourId, YachtTourNonOperationDaySearchModel searchModel)
        {
            try
            {
                var query = _db.YachtTourNonOperationDays
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.YachtTourFid == tourId)
                    .Select(k => _mapper.Map<YachtTourNonOperationDays, YachtTourNonOperationDayViewModel>(k));
                var totlaItems = query.Count();

                var queryData = query.Skip(searchModel.PageSize * (searchModel.PageIndex - 1))
                  .Take(searchModel.PageSize)
                  .ToList();

                if (queryData != null)
                {
                    var data = queryData.Select(k=> LoadYachtName(k)).ToList();
                    var pagedList = new PagedList<YachtTourNonOperationDayViewModel>(data, totlaItems, searchModel.PageIndex, searchModel.PageSize);
                    return BaseResponse<PagedList<YachtTourNonOperationDayViewModel>>.Success(pagedList);
                }
                    
                return BaseResponse<PagedList<YachtTourNonOperationDayViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtTourNonOperationDayViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourNonOperationDayViewModel>> FindByYachtFid(int yachtId)
        {
            try
            {
                var result = _db.YachtTourNonOperationDays
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.YachtFid == yachtId)
                    .Select(k => _mapper.Map<YachtTourNonOperationDays, YachtTourNonOperationDayViewModel>(k));
                var data = result.ToList();
                if (data != null)
                    return BaseResponse<List<YachtTourNonOperationDayViewModel>>.Success(LoadYachtName(data));
                return BaseResponse<List<YachtTourNonOperationDayViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourNonOperationDayViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourNonOperationDayViewModel> FindById(int id)
        {
            try
            {
                var data = _db.YachtTourNonOperationDays
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.Id == id)
                    .Select(k => _mapper.Map<YachtTourNonOperationDays, YachtTourNonOperationDayViewModel>(k))
                    .FirstOrDefault();

                if (data != null)
                    return BaseResponse<YachtTourNonOperationDayViewModel>.Success(LoadYachtName(data));
                return BaseResponse<YachtTourNonOperationDayViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourNonOperationDayViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var entity = GenerateUpdateModel(_db.YachtTourNonOperationDays
                                .FirstOrDefault(k => !k.Deleted && k.Id == id));
                if(entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.Deleted = true;
                _db.Update(entity);
                var save = _db.SaveChanges();
                return save > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        private YachtTourNonOperationDays GenerateCreateModel(YachtTourNonOperationDayCreateModel model)
        {
            try
            {
                var entity = _mapper.Map<YachtTourNonOperationDayCreateModel, YachtTourNonOperationDays>(model);
                var userId = UserContextHelper.UserId;
                var now = DateTime.Now.Date;
                entity.Deleted = false;
                entity.CreatedDate = now;
                entity.LastModifiedDate = now;
                entity.CreatedBy = userId;
                entity.LastModifiedBy = userId;
                return entity;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private YachtTourNonOperationDays GenerateUpdateModel(YachtTourNonOperationDays entity)
        {
            try
            {
                var userId = UserContextHelper.UserId;
                var now = DateTime.Now.Date;
                entity.LastModifiedDate = now;
                entity.LastModifiedBy = userId;
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<YachtTourNonOperationDayViewModel> LoadYachtName(List<YachtTourNonOperationDayViewModel> models)
        {
            foreach (var item in models)
            {
                var yachtId = item.YachtFid.Decrypt().ToInt32();
                var yacht = _yachtService.GetYachtBasicProfile(yachtId);
                if (yacht != null)
                {
                    item.YachtName = yacht.ResponseData?.YachtName;
                }
            }
            return models;
        }

        private YachtTourNonOperationDayViewModel LoadYachtName(YachtTourNonOperationDayViewModel model)
        {
            var yachtId = model.YachtFid.Decrypt().ToInt32();
            var yacht = _yachtService.GetYachtBasicProfile(yachtId);
            if (yacht != null)
            {
                model.YachtName = yacht.ResponseData?.YachtName;
            }
            return model;
        }
    }
}