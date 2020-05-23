using System;
using AQEncrypts;
using AutoMapper;
using System.Linq;
using Omu.ValueInjecter;
using APIHelpers.Response;
using Identity.Core.Helpers;
using AQBooking.Core.Helpers;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Interfaces;
using AQConfigurations.Core.Services.Interfaces;
using YachtMerchant.Core.Models.YachtTourPricings;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourPricingService : IYachtTourPricingService
    {
        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        private readonly IYachtService _yachtService;
        private readonly ICommonValueRequestService _commonValueRequestService;

        public YachtTourPricingService(YachtOperatorDbContext db,
            IMapper mapper,
            IYachtService yachtService,
            ICommonValueRequestService commonValueRequestService)
        {
            _db = db;
            _mapper = mapper;
            _yachtService = yachtService;
            _commonValueRequestService = commonValueRequestService;
        }

        public BaseResponse<bool> Create(int tourId, int yachtId, YachtTourPricingCreateModel model)
        {
            try
            {
                if (tourId < 1 || yachtId < 1 || model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = GenerateForCreate(tourId, yachtId, model);
                _db.YachtTourPricings.Add(entity);
                var save = _db.SaveChanges();
                return save > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtTourPricingUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var entity = GenerateForUpdate(model);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                _db.Update(entity);
                var save = _db.SaveChanges();
                return save > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(List<YachtTourPricingUpdateModel> models)
        {
            try
            {
                if (models == null)
                    return BaseResponse<bool>.BadRequest();
                var entities = models.Select(k => GenerateForUpdate(k)).ToList();
                if (entities == null)
                    return BaseResponse<bool>.BadRequest();
                _db.UpdateRange(entities);
                var save = _db.SaveChanges();
                return save > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(long id)
        {
            try
            {
                if (id < 1)
                    return BaseResponse<bool>.BadRequest();
                var entity = Find(id);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                var userId = UserContextHelper.UserId;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now.Date;
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

        public BaseResponse<PagedList<YachtTourPricingViewModel>> Search(int tourId, YachtTourPricingSearchModel model)
        {
            try
            {
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var query = (from i in _db.YachtTourPricings
                             join y in _db.Yachts on i.YachtFid equals y.Id
                             where !i.Deleted && !y.Deleted && i.TourFid == tourId
                             select new YachtTourPricingViewModel()
                             {
                                 Id = i.Id,
                                 YachtFid = Terminator.Encrypt(i.YachtFid.ToString()),
                                 YachtName = y.Name,
                                 TourFid = Terminator.Encrypt(i.TourFid.ToString()),
                                 TourPricingTypeResKey = i.TourPricingTypeResKey,
                                 EffectiveDate = i.EffectiveDate,
                                 EffectiveEndDate = i.EffectiveEndDate,
                                 TourFee = i.TourFee,
                             }).OrderBy(x => x.YachtName).ThenBy(x => x.EffectiveDate);
                var result = new PagedList<YachtTourPricingViewModel>(query, pageIndex, pageSize);
                if (result != null)
                    return BaseResponse<PagedList<YachtTourPricingViewModel>>.Success(result);
                return BaseResponse<PagedList<YachtTourPricingViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtTourPricingViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtTourPricingDetailModel>> SearchDetail(int tourId, YachtTourPricingSearchModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<PagedList<YachtTourPricingDetailModel>>.BadRequest();
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "YachtName";
                var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "DESC";
                var sortString = $"{sortColumn} {sortType}";
                var query = (from o in _db.YachtTourOperationDetails.Where(k => !k.Deleted && k.TourFid == tourId)
                            join y in _db.Yachts.Where(k => !k.Deleted) on o.YachtFid equals y.Id
                            select new YachtBasicProfileModel() {
                                YachtId = y.Id,
                                YachtUniqueId = y.UniqueId,
                                MerchantId = y.MerchantFid,
                                YachtName = y.Name
                            }).OrderBy(sortString);
                var allYachsOfTour = query.ToList();
                var allPricingDetails = new List<YachtTourPricingDetailModel>();
                foreach (var yacht in allYachsOfTour)
                {
                    var currentPricing = GetCurrentPricingByTourIdAndYachtId(tourId, yacht.YachtId);
                    if(currentPricing != null)
                    {
                        allPricingDetails.Add(new YachtTourPricingDetailModel()
                        {
                            Yacht = yacht,
                            CurrentPricing = currentPricing
                        });
                    }
                }
                var totalItems = allPricingDetails.Count();
                var result = allPricingDetails
                                .Skip(pageSize * (pageIndex - 1))
                                .Take(pageSize)
                                .ToList();
                var pagedList = new PagedList<YachtTourPricingDetailModel>(result, totalItems, pageIndex, pageSize);
                return BaseResponse<PagedList<YachtTourPricingDetailModel>>.Success(pagedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtTourPricingDetailModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourPricingDetailModel>> GetDetailByTourId(int tourId)
        {
            try
            {
                if (tourId < 1)
                    return BaseResponse<List<YachtTourPricingDetailModel>>.BadRequest();
                var listTourPricings = _db.YachtTourPricings
                                  .AsNoTracking()
                                  .Where(k => !k.Deleted && k.TourFid == tourId)
                                  .ToList();
                var listYachtIds = listTourPricings
                                   .Select(k => k.YachtFid)
                                   .Distinct()
                                   .ToList();
                var listPricingDetails = new List<YachtTourPricingDetailModel>();
                foreach (var yachtId in listYachtIds)
                {
                    var yacht = _yachtService.GetYachtBasicProfile(yachtId);
                    if (yacht != null)
                    {
                        var currentPricing = listTourPricings
                                                 .Where(k => k.YachtFid == yachtId && (k.EffectiveDate.Date <= DateTime.Now.Date))
                                                 .Select(k => _mapper.Map<YachtTourPricings, YachtTourPricingViewModel>(k))
                                                 .OrderByDescending(k=> k.EffectiveDate)
                                                 .FirstOrDefault();
                        listPricingDetails.Add(new YachtTourPricingDetailModel()
                        {
                            Yacht = yacht.ResponseData,
                            CurrentPricing = currentPricing
                        });
                    }
                }

                return BaseResponse<List<YachtTourPricingDetailModel>>.Success(listPricingDetails);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourPricingDetailModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourPricingViewModel> GetById(long id)
        {
            try
            {
                if (id < 1)
                    return BaseResponse<YachtTourPricingViewModel>.BadRequest();
                var entity = Find(id);
                if (entity == null)
                    return BaseResponse<YachtTourPricingViewModel>.BadRequest();
                var model = _mapper.Map<YachtTourPricings, YachtTourPricingViewModel>(entity);
                return model != null ? BaseResponse<YachtTourPricingViewModel>.Success(model) : BaseResponse<YachtTourPricingViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourPricingViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourPricingViewModel>> GetByTourIdAndYachtId(int tourId, int yachtId)
        {
            try
            {
                if (tourId < 1 || yachtId < 1)
                    return BaseResponse<List<YachtTourPricingViewModel>>.BadRequest();
                var entiies = FindByTourIdAndYachtId(tourId, yachtId);
                if (entiies == null)
                    return BaseResponse<List<YachtTourPricingViewModel>>.BadRequest();
                var models = entiies.Select(k => _mapper.Map<YachtTourPricings, YachtTourPricingViewModel>(k)).ToList();
                return models != null
                    ? BaseResponse<List<YachtTourPricingViewModel>>.Success(models)
                    : BaseResponse<List<YachtTourPricingViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourPricingViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region Private Methods

        private YachtTourPricingViewModel GetCurrentPricingByTourIdAndYachtId(int tourId, int yachtId)
        {
            var currentPricing = _db.YachtTourPricings
                .Where(k => !k.Deleted && k.TourFid == tourId && k.YachtFid == yachtId)
                .Where(k => k.EffectiveDate <= DateTime.Now.Date)
                .OrderByDescending(k=> k.EffectiveDate)
                 .Select(k=> _mapper.Map<YachtTourPricingViewModel>(k))
                .FirstOrDefault();
            return currentPricing;
        }

        private YachtTourPricings GenerateForCreate(int tourId, int yachtId, YachtTourPricingCreateModel model = null)
        {
            var pricingTypeResponse = _commonValueRequestService.
                GetCommonValueByGroupInt(CommonValueGroupConstant.YachtTourPricingType, model.TourPricingTypeFid);
            var entity = new YachtTourPricings();
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            if (model != null)
                entity.InjectFrom(model);
            entity.TourFid = tourId;
            entity.YachtFid = yachtId;
            entity.TourPricingTypeResKey = pricingTypeResponse.IsSuccessStatusCode ? pricingTypeResponse.ResponseData.ResourceKey : string.Empty;
            entity.CreatedBy = userId;
            entity.LastModifiedBy = userId;
            entity.CreatedDate = now;
            entity.LastModifiedDate = now;
            entity.Deleted = false;
            return entity;
        }

        private YachtTourPricings GenerateForUpdate(YachtTourPricingUpdateModel model = null)
        {
            var entity = Find(model.Id);
            if (entity == null)
                return null;
            var pricingTypeResponse = _commonValueRequestService.GetCommonValueByGroupInt(CommonValueGroupConstant.YachtTourPricingType, model.TourPricingTypeFid);
            var now = DateTime.Now.Date;
            var userId = UserContextHelper.UserId;
            entity.LastModifiedBy = userId;
            entity.LastModifiedDate = now;
            entity.InjectFrom(model);
            entity.TourPricingTypeResKey = pricingTypeResponse.IsSuccessStatusCode ? pricingTypeResponse.ResponseData.ResourceKey : string.Empty;

            return entity;
        }

        private YachtTourPricings Find(long id)
        {
            var entity = _db.YachtTourPricings
                            .AsNoTracking()
                            .FirstOrDefault(k => !k.Deleted && k.Id == id);
            return entity;
        }

        private List<YachtTourPricings> FindByTourIdAndYachtId(int tourId, int yachtId)
        {
            var entities = _db.YachtTourPricings
                              .Where(k => !k.Deleted && k.TourFid == tourId && k.YachtFid == yachtId)
                              .ToList();
            return entities;
        }

        #endregion Private Methods
    }
}