using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using Identity.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Core.Models.YachtTours;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using YachtMerchant.Infrastructure.RequestServices.Interfaces;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourServices : IYachtToursServices
    {
        private const string DECRYPT_FAIL_VALUE = "0";

        #region Dependence Injection

        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        private readonly IYachtService _yachtService;
        private readonly ILocationRequestService _locationRequestService;
        private readonly IYachtTourOperationDetailService _yachtTourOperationDetailService;

        #endregion Dependence Injection

        public YachtTourServices(YachtOperatorDbContext db,
            IYachtService yachtService,
            ILocationRequestService locationRequestService,
            IYachtTourOperationDetailService yachtTourOperationDetailService,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _yachtService = yachtService;
            _locationRequestService = locationRequestService;
            _yachtTourOperationDetailService = yachtTourOperationDetailService;
        }

        private DateTime? StringToDate(string dateString)
        {
            try
            {
                return DateTime.Parse(dateString);
            }
            catch
            {
                return null;
            }
        }

        public BaseResponse<YachtTourCheckActiveModel> ActiveTour(int tourId)
        {
            try
            {
                var tour = _db.YachtTours.FirstOrDefault(x => x.Id == tourId && !x.Deleted);

                if (tour == null)
                    return BaseResponse<YachtTourCheckActiveModel>.BadRequest();

                var infor = _db.YachtTourInformations.Any(x => x.TourFid == tourId && !x.Deleted && x.IsActivated == true 
                                        && x.ActivatedDate.Value.Date <= DateTime.Now.Date);
                var tourOperationDetail = _db.YachtTourOperationDetails.Any(x=>x.TourFid == tourId && !x.Deleted 
                                        && x.IsActive == true && x.EffectiveDate.Date <= DateTime.Now.Date);
                var tourPricing = _db.YachtTourPricings.Any(x => x.TourFid == tourId 
                                        && !x.Deleted && x.EffectiveDate.Date <= DateTime.Now.Date);
                var tourRefImage = _db.YachtTourFileStreams.Any(x => x.YachtTourFid == tourId
                                        && !x.Deleted && x.ActivatedDate.Date <= DateTime.Now.Date && x.FileTypeFid == 11);

                var tourImage = _db.YachtTourFileStreams.Any(x => x.YachtTourFid == tourId
                                        && !x.Deleted && x.ActivatedDate.Date <= DateTime.Now.Date && x.FileTypeFid == 10
                                        && (x.FileCategoryFid == 1 || x.FileCategoryFid == 2 || x.FileCategoryFid ==3));

                var model = new YachtTourCheckActiveModel()
                {
                    CheckTourInfo = infor,
                    CheckTourOperationDetail = tourOperationDetail,
                    CheckTourPricing = tourPricing,
                    CheckTourImage = tourImage,
                    CheckTourRefImage =tourRefImage,
                    CheckActiveTour = tour.IsActive,
                };

                if (infor && tourOperationDetail && tourPricing && tourRefImage && tourImage)
                    model.Allow = true;

                return BaseResponse<YachtTourCheckActiveModel>.Success(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourCheckActiveModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachTourDetailModel>> Search(int merchantId, YachtTourSearchModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<PagedList<YachTourDetailModel>>.BadRequest();
                var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "TourName";
                var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "ASC";
                var sortString = $"{sortColumn} {sortType}";
                DateTime effStartDate;
                DateTime effEndDate;
                var tourName = !string.IsNullOrEmpty(model.TourName) ? model.TourName.ToUpper() : string.Empty;
                var effStartDateIsValid = DateTime.TryParse(model.EffectiveStartDate, out effStartDate);
                var effEndDateIsValid = DateTime.TryParse(model.EffectiveEndDate, out effEndDate);
                var query = _db.YachtTours
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.MerchantFid == merchantId
                               && (string.IsNullOrEmpty(tourName) || k.TourName.ToUpper().Contains(tourName))
                               && (!effStartDateIsValid || k.EffectiveDate.Date >= effStartDate)
                               && (!effEndDateIsValid || k.EffectiveEndDate <= effEndDate));
                var totalItems = query.Count();
                var listTour = query
                              .Skip(pageSize * (pageIndex - 1))
                              .Take(pageSize)
                              .OrderBy(sortString)
                              .ToList()
                              .Select(k => new YachTourDetailModel()
                              {
                                  Tour = _mapper.Map<YachtTours, YachTourViewModel>(k),
                                  ListYachts = GetListYachtByTourId(k.Id)
                              })
                              .ToList();
                var pagedList = new PagedList<YachTourDetailModel>(listTour, totalItems, pageIndex, pageSize);
                return BaseResponse<PagedList<YachTourDetailModel>>.Success(pagedList);
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachTourDetailModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> Create(YachtTourCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var tour = GenerateYachtTourFromCreateModel(model);
                    if (tour == null)
                        return BaseResponse<string>.BadRequest();
                    _db.YachtTours.Add(tour);
                    _db.SaveChanges();

                    var counter = new YachtTourCounters();
                    counter.YachtTourId = tour.Id;
                    counter.YachtTourUniqueId = UniqueIDHelper.GenarateRandomString(12);
                    counter.TotalViews = 1000;
                    counter.TotalBookings = 100;
                    counter.TotalSuccessBookings = 1000;
                    counter.TotalReviews = 10000;
                    counter.TotalRecommendeds = 100;
                    counter.TotalNotRecommendeds = 1;
                    _db.YachtTourCounters.Add(counter);
                    _db.SaveChanges();

                    transaction.Commit();
                    return BaseResponse<string>.Success(Terminator.Encrypt(tour.Id.ToString()));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }

        public BaseResponse<List<YachTourViewModel>> GetToursByMerchantFid(int merchantId)
        {
            try
            {
                var models = _db.YachtTours
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.MerchantFid == merchantId)
                    .Select(k => _mapper.Map<YachtTours, YachTourViewModel>(k))
                    .ToList();

                return BaseResponse<List<YachTourViewModel>>.Success(models);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachTourViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachTourDetailModel>> GetTourDetailsByMerchantFid(int merchantId)
        {
            try
            {
                var models = _db.YachtTours
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.MerchantFid == merchantId)
                    .Select(k => new YachTourDetailModel()
                    {
                        Tour = _mapper.Map<YachtTours, YachTourViewModel>(k),
                        //ListYachts = GetListYachtByTourId(k.Id)
                    })
                    .ToList();

                foreach (var item in models)
                {
                    var tourId = item.Tour.Id.Decrypt().ToInt32();
                    item.ListYachts = GetListYachtByTourId(tourId);
                    item.TourCharterCount = _db.YachtTourCharters.Where(k => k.YachtTourFid == tourId).Count();
                }

                return BaseResponse<List<YachTourDetailModel>>.Success(models);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachTourDetailModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachTourViewModel> GetTourById(int yachTourId)
        {
            try
            {
                var tour = Find(yachTourId);
                if (tour == null)
                    return BaseResponse<YachTourViewModel>.BadRequest();
                var model = _mapper.Map<YachtTours, YachTourViewModel>(tour);
                return BaseResponse<YachTourViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachTourViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtTourSelectListModel>> GetYachtTourSelectItem()
        {
            try
            {
                var yachtTourSelect = _db.YachtTours.Where(x => !x.Deleted).OrderBy(c => c.TourName)
                    .Select(s => new YachtTourSelectListModel()
                    {
                        Id = s.Id.ToString(),
                        TourName = s.TourName,
                        UniqueId = s.UniqueId
                    }).ToList();
                return BaseResponse<List<YachtTourSelectListModel>>.Success(yachtTourSelect);
            }
            catch
            {
                throw;
            }
        }

        public BaseResponse<bool> Update(YachtTourUpdateModel model, int tourId)
        {
            try
            {
                var entity = Find(tourId);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();

                //entity.InjectFrom(model);
               entity = _mapper.Map<YachtTourUpdateModel, YachtTours>(model, entity);
                entity = GenerateForUpdate(entity);
                _db.Update(entity);
                return _db.SaveChanges() > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Delete(int tourId)
        {
            try
            {
                var entity = Find(tourId);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity = GenerateForUpdate(entity);
                entity.Deleted = true;
                _db.Update(entity);
                return _db.SaveChanges() > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Activate(int tourId)
        {
            try
            {
                var entity = Find(tourId);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity = GenerateForUpdate(entity);
                entity.IsActive = true;
                _db.Update(entity);
                return _db.SaveChanges() > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Deactivate(int tourId)
        {
            try
            {
                var entity = Find(tourId);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity = GenerateForUpdate(entity);
                entity.IsActive = false;
                _db.Update(entity);
                var save = _db.SaveChanges();
                return save > 0 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region Private Methods

        private YachtTours GenerateYachtTourFromCreateModel(YachtTourCreateModel model)
        {
            var cityRespone = _locationRequestService.GetCityNameById(model.CityFid);
            var countryRespone = _locationRequestService.GetCountryNameById(model.CountryFid);
            var locationRespone = _locationRequestService.GetLocationNameById(model.CountryFid);

            var tour = new YachtTours();
            //tour.InjectFrom(model);
            //tour.TourDurationValue = model.TourDurationValue;
            tour = _mapper.Map<YachtTourCreateModel, YachtTours>(model, tour);
            //tour.TourDurationUnitResKey = (tour.TourDurationUnitTypeFid > 0) ? "TourDurationUnitResKey" : string.Empty;
            tour.City = cityRespone.ResponseData ?? string.Empty;
            tour.Country = countryRespone.ResponseData ?? string.Empty;
            tour.LocationName = locationRespone.ResponseData ?? string.Empty;

            var userId = UserContextHelper.UserId;
            var now = DateTime.Now.Date;
            tour.UniqueId = UniqueIDHelper.GenarateRandomString(12);
            tour.Deleted = false;
            tour.IsActive = false;
            tour.CreatedDate = now;
            tour.LastModifiedDate = now;
            tour.CreatedBy = userId;
            tour.LastModifiedBy = userId;
            tour.UniqueId = UniqueIDHelper.GenarateRandomString(12);
            return tour;
        }

        private YachtTours GenerateForUpdate(YachtTours tour)
        {
            var userId = UserContextHelper.UserId;
            var now = DateTime.Now.Date;
            tour.LastModifiedDate = now;
            tour.LastModifiedBy = userId;
            return tour;
        }

        private YachtTours Find(int key) => _db.YachtTours.AsNoTracking().FirstOrDefault(k => !k.Deleted && key == k.Id);

        private List<YachtBasicProfileModel> GetListYachtByTourId(int tourId)
        {
            var listTours = _yachtTourOperationDetailService.FindByTourId(tourId);

            var listYacht = listTours.IsSuccessStatusCode
                            ? listTours.ResponseData
                                .Select(k => _yachtService.GetYachtBasicProfile(k.YachtFid.Decrypt().ToInt32())?.ResponseData)
                                .ToList()
                            : new List<YachtBasicProfileModel>();
            return listYacht;
        }

        #endregion Private Methods
    }
}